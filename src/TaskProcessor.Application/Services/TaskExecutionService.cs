
using TaskProcessor.Application.Services;

namespace TaskProcessor.Domain.Model
{
    public class TaskExecutionService
    {
        private readonly TaskService _taskService;
        private readonly SubTaskService _subtaskService;
        private readonly object _lockObject = new object();

        public TaskExecutionService(TaskService taskService, SubTaskService subTaskService)
        {
            _taskService = taskService;
            _subtaskService = subTaskService;
        }

        public async Task ExecuteTopTasksWithSubTasksAsync(int topTasksCount)
        {
            while (true)
            {
                var tasksByPriority = GetTopTasksByPriority(topTasksCount);
                var executionTasks = tasksByPriority.Select(task => Task.Run(() => ExecuteTaskAsync(task))).ToList();
                
                await Task.WhenAll(executionTasks);

                if (NoMoreTasksToProcess()) 
                {
                    break;
                }
            }
        }

        private bool NoMoreTasksToProcess()
        {
            var allTasks = _taskService.GetAllTasks();
            var pendingTasks = allTasks.Any(task => (int) task.Status <= 3);
            return !pendingTasks;
        }

        private async Task ExecuteTaskAsync(TaskEntity task)
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

        private async Task ExecuteSubTaskAsync(SubTaskEntity subTask, TaskEntity parentTask)
        {
            ServiceHelper.LogStartSubTask(subTask);
            subTask.StartSubTask();

            while (!IsCompleted(subTask))
            {
                TryUpdatingElapsedTime(subTask);
                TryCompletingMainTask(subTask, parentTask);
            }
            ServiceHelper.LogTaskProgress(parentTask);
            ServiceHelper.LogCompleteSubTask(subTask);
        }

        private bool IsCompleted<T>(T entity)
        {
            return entity != null && entity.GetType().GetProperty("Status")?.GetValue(entity) is TaskStatusEnum.Completed;
        }

        private void TryUpdatingElapsedTime(SubTaskEntity subTask)
        {
            lock (_lockObject)
            {
                subTask.UpdateElapsedTime();
                _subtaskService.Update(subTask);
                ServiceHelper.LogUpdateSubTask(subTask);
            }
        }

        private void TryCompletingMainTask(SubTaskEntity subTask, TaskEntity parentTask)
        {
            if (subTask.Status == TaskStatusEnum.Completed)
            {
                parentTask.CompletedSubTasks++;

                if (parentTask.CompletedSubTasks == parentTask.TotalSubTasks)
                {
                    parentTask.Status = TaskStatusEnum.Completed;
                    lock (_lockObject)
                    {
                        _taskService.Update(parentTask);
                    }
                }
            }
        }

        private IEnumerable<TaskEntity> GetTopTasksByPriority(int topTasksCount)
        {
            return _taskService
                .GetAllTasksByPriorityAndNumberOfSubTasks()
                .Where(task => (int)task.Status <= 3)
                .Take(topTasksCount)
                .ToList();
        }
    }
}
