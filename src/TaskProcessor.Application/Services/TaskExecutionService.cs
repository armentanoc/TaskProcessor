
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

            var subTaskExecutionTasks = task.SubTasks
            .OrderByDescending(subTask => subTask.ElapsedTime)
            .Select(subTask => Task.Run(() => ExecuteSubTaskAsync(subTask, task)))
            .ToList();
            
            Log($"Task Completed: {GetTaskInformation(task)}");
        }

        private string GetTaskInformation(TaskEntity task)
        {
            return $"Id {task.Id} - Priority {task.Priority} " +
                $"\nSubtasks: {string.Join(", ", task.SubTasks.Select(subTask => subTask.Id))}";
            //$"SubTask Id: {subTask.Id}, Duration: {subTask.Duration.TotalSeconds}s"))} {GetDateTime()}";
        }

        private string GetDateTime()
        {
            return $"At {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
        }

        private async Task ExecuteSubTaskAsync(SubTaskEntity subTask, TaskEntity parentTask)
        {
            Log($"\nStarting SubTask: {GetSubTaskInformation(subTask)}");

            subTask.StartSubTask();

            while (!IsCompleted(subTask))
            {
                TryUpdatingElapsedTime(subTask);
                Log($"\nSubTask Updated: {GetSubTaskInformation(subTask)}");
                TryCompletingMainTask(subTask, parentTask);
            }
            Log($"\nSubTask Completed: {GetSubTaskInformation(subTask)}");
            Log($"Progress: {parentTask.CompletedSubTasks}/{parentTask.TotalSubTasks} subtasks completed for Task {parentTask.Id}");
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
            if (!IsCompleted(parentTask))
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

        private bool IsCompleted(TaskEntity task)
        {
            return task != null && task.Status == TaskStatusEnum.Completed;
        }

        private bool IsCompleted(SubTaskEntity task)
        {
            return task != null && task.Status == TaskStatusEnum.Completed;
        }

        private object GetSubTaskInformation(SubTaskEntity subTask)
        {
            return
                $" \nTaskId: {subTask.TaskId}" +
                $" SubTaskId: {subTask.Id} {GetDateTime()}" +
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
