
using System.Net.NetworkInformation;
using System.Threading.Tasks;
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
            if (task.Status == TaskStatusEnum.Completed || task.Status == TaskStatusEnum.Cancelled)
            {
                return;
            } else if(task.Status == TaskStatusEnum.InProgress)
            {
                Log($"\nRestarting Task: {GetTaskInformation(task)}");
            } else
            {
                Log($"\nStarting Task: {GetTaskInformation(task)}");
                AlterStatusToInProgress(task);
            }

            var subTaskExecutionTasks = task.SubTasks
            .OrderByDescending(subTask => subTask.ElapsedTime)
            .Select(subTask => Task.Run(() => ExecuteSubTaskAsync(subTask, task)))
            .ToList();
            
        }

        private void AlterStatusToInProgress(TaskEntity task)
        {
            task.Status = TaskStatusEnum.InProgress;
            lock (_lockObject)
            {
                _taskService.Update(task);
            }
        }

        private void AlterStatusToInProgress(SubTaskEntity task)
        {
            task.Status = TaskStatusEnum.InProgress;
            lock (_lockObject)
            {
                _subtaskService.Update(task);
            }
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
            AlterStatusToInProgress(subTask);

            while (!IsCompleted(subTask))
            {
                TryUpdatingElapsedTime(subTask, parentTask);
                Log($"\nSubTask Updated: {GetSubTaskInformation(subTask)}");
                TryCompletingMainTask(subTask, parentTask);
            }
        }

        private void TryUpdatingElapsedTime(SubTaskEntity subTask, TaskEntity parentTask)
        {
            lock (_lockObject)
            {
                subTask.UpdateElapsedTime();
                _subtaskService.Update(subTask);

                if (subTask.ElapsedTime >= subTask.Duration)
                {
                    subTask.Status = TaskStatusEnum.Completed;
                    parentTask.CompletedSubTasks++;
                    Log($"\nSubTask Completed: {GetSubTaskInformation(subTask)}");
                    Log($"Progress: {parentTask.CompletedSubTasks}/{parentTask.TotalSubTasks} subtasks completed for Task {parentTask.Id}");
                }
            }
        }

        private void TryCompletingMainTask(SubTaskEntity subTask, TaskEntity parentTask)
        {
            if (!IsCompleted(parentTask))
            {
                if (parentTask.CompletedSubTasks == parentTask.TotalSubTasks)
                {
                    parentTask.Status = TaskStatusEnum.Completed;
                    lock (_lockObject)
                    {
                        _taskService.Update(parentTask);
                        Log($"Task Completed: {GetTaskInformation(parentTask)}");
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
