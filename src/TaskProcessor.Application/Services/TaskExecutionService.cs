
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
            var tasksByPriority = GetTopTasksByPriority(topTasksCount);
            var executionTasks = tasksByPriority.Select(task => Task.Run(() => ExecuteTaskAsync(task))).ToList();
            await Task.WhenAll(executionTasks);
        }
        private async Task ExecuteTaskAsync(TaskEntity task)
        {
            Log($"\nExecuting Task: {GetTaskInformation(task)}");
            var subTaskExecutionTasks = task.SubTasks.Select(subTask => Task.Run(() => ExecuteSubTaskAsync(subTask, task))).ToList();
            await Task.WhenAll(subTaskExecutionTasks);
            Log($"Task Completed: {GetTaskInformation(task)}");
        }

        private string GetTaskInformation(TaskEntity task)
        {
            return $"Id {task.Id} - Priority {task.Priority} \nSubtasks: {string.Join(", ", task.SubTasks.Select(subTask => $"SubTask Id: {subTask.Id}, Duration: {subTask.Duration.TotalSeconds}s"))} {GetDateTime()}";
        }

        private string GetDateTime()
        {
            return $"At {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
        }

        private async Task ExecuteSubTaskAsync(SubTaskEntity subTask, TaskEntity parentTask)
        {
            Log($"\nExecuting SubTask: {GetSubTaskInformation(subTask)}");

            subTask.StartSubTask();

            while (!IsCompleted(subTask))
            {
                TryUpdatingElapsedTime(subTask);
                Log($"\nSubTask Updated: {GetSubTaskInformation(subTask)}");
                TryCompletingMainTask(subTask, parentTask);
            }
            Log($"Progress: {parentTask.CompletedSubTasks}/{parentTask.TotalSubTasks} subtasks completed for Task {parentTask.Id}");
            Log($"\nSubTask Completed: {GetSubTaskInformation(subTask)}");
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

        private object GetSubTaskInformation(SubTaskEntity subTask)
        {
            return 
                $"{subTask.Id} {GetDateTime()}" +
                $" Duration: {subTask.Duration}" +
                $" ElapsedTime: {subTask.ElapsedTime}";
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }

        private IEnumerable<TaskEntity> GetTopTasksByPriority(int topTasksCount)
        {
            return _taskService
                .GetAllTasksByPriorityAndNumberOfSubTasks()
                .Take(topTasksCount)
                .ToList();
        }
    }
}
