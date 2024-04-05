using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Services
{
    public class TaskExecutionService : ITaskExecutionService
    {
        private readonly ITaskService _taskService;
        private readonly ISubTaskService _subtaskService;
        private readonly object _lockObject = new object();

        public TaskExecutionService(ITaskService taskService, ISubTaskService subTaskService)
        {
            _taskService = taskService;
            _subtaskService = subTaskService;
        }

        public async Task ExecuteTopTasksWithSubTasksAsync(int topTasksCount, int idOfTaskToPause = -1)
        {
            while (true)
            {
                var tasksByPriority = GetTopTasksByPriority(topTasksCount).Where(task => task.Id != idOfTaskToPause);
                var executionTasks = tasksByPriority.Select(task => Task.Run(() => ExecuteTaskAsync(task))).ToList();

                await Task.WhenAll(executionTasks);

                if (NoMoreTasksToProcess())
                    break;
            }
        }

        public bool NoMoreTasksToProcess()
        {
            var allTasks = _taskService.GetAllTasks();
            var pendingTasks = allTasks.Any(task => (int)task.Status <= 3);
            return !pendingTasks;
        }

        public async Task ExecuteTaskAsync(TaskEntity task)
        {
            ServiceHelper.LogStartTask(task);

            var subTaskExecutionTasks =
                task.SubTasks
                .Select(subTask => Task.Run(() => ExecuteSubTaskAsync(subTask, task)))
                .ToList()
                .Where(subTask => (int)subTask.Status <= 3);

            await Task.WhenAll(subTaskExecutionTasks);

            ServiceHelper.LogCompleteTask(task);
        }

        public async Task ExecuteSubTaskAsync(SubTaskEntity subTask, TaskEntity parentTask)
        {
            ServiceHelper.LogStartSubTask(subTask);
            subTask.StartSubTask();

            parentTask.Begin();

            lock (_lockObject)
                _taskService.Update(parentTask);

            while (!IsCompleted(subTask))
            {
                TryUpdatingElapsedTime(subTask);
                TryCompletingMainTask(subTask, parentTask);
            }
            ServiceHelper.LogTaskProgress(parentTask);
            ServiceHelper.LogCompleteSubTask(subTask);
        }

        public bool IsCompleted<T>(T entity)
        {
            return entity != null && entity.GetType().GetProperty("Status")?.GetValue(entity) is TaskStatusEnum.Completed;
        }

        public void TryUpdatingElapsedTime(SubTaskEntity subTask)
        {
            lock (_lockObject)
            {
                subTask.UpdateElapsedTime();
                _subtaskService.Update(subTask);
                ServiceHelper.LogUpdateSubTask(subTask);
            }
        }

        public void TryCompletingMainTask(SubTaskEntity subTask, TaskEntity parentTask)
        {
            if (subTask.Status == TaskStatusEnum.Completed)
            {
                parentTask.CompletedSubTasks++;

                if (parentTask.CompletedSubTasks == parentTask.TotalSubTasks)
                {
                    parentTask.Status = TaskStatusEnum.Completed;

                    lock (_lockObject)
                        _taskService.Update(parentTask);
                }
            }
        }

        public IEnumerable<TaskEntity> GetTopTasksByPriority(int topTasksCount)
        {
            return _taskService
                .GetAllTasksByPriorityAndNumberOfSubTasks()
                .Where(task => (int)task.Status <= 3)
                .Take(topTasksCount)
                .ToList();
        }
    }
}
