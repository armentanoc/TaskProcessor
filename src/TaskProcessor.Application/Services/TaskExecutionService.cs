
using TaskProcessor.Application.Services;

namespace TaskProcessor.Domain.Model
{
    public class TaskExecutionService
    {
        private readonly TaskService _taskService;
        private readonly SubTaskService _subtaskService;
        //private readonly object _lockObject = new object();

        public TaskExecutionService(TaskService taskService, SubTaskService subTaskService)
        {
            _taskService = taskService;
            _subtaskService = subTaskService;
        }

        public async Task ExecuteTopTasksWithSubTasksAsync(int topTasksCount)
        {
            var tasksByPriority = GetTopTasksByPriority(topTasksCount);
            var executionTasks = tasksByPriority.Select(task => ExecuteTaskAsync(task)).ToList();
            await Task.WhenAll(executionTasks);
        }

        private async Task ExecuteTaskAsync(TaskEntity task)
        {
            Log($"\n({task.Priority}) Executing Task: {task.Id} with subtasks: {string.Join(", ", task.SubTasks.Select(subTask => $"SubTask Id: {subTask.Id}, Duration: {subTask.Duration.TotalSeconds}s"))} (StartTime: at {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")})");
            var subTaskExecutionTasks = task.SubTasks.Select(subTask => ExecuteSubTaskAsync(subTask, task)).ToList();
            await Task.WhenAll(subTaskExecutionTasks);
            Log($"Task Completed: {task.Id} at {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
        }

        private async Task ExecuteSubTaskAsync(SubTaskEntity subTask, TaskEntity parentTask)
        {
            Log($"\nExecuting SubTask: {subTask.Id} (StartTime: at {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")})" +
                $" Duration: {subTask.Duration}" +
                $" ElapsedTime: {subTask.ElapsedTime}");

            subTask.StartSubTask();

            while (subTask.Status != TaskStatusEnum.Completed)
            {
                subTask.UpdateElapsedTime();
                _subtaskService.Update(subTask);

                Log($"SubTask Updated: {subTask.Id}" +
                    $" Duration: {subTask.Duration}" +
                    $" ElapsedTime: {subTask.ElapsedTime}");

                if (subTask.Status == TaskStatusEnum.Completed)
                {
                    parentTask.CompletedSubTasks++;

                    if (parentTask.CompletedSubTasks == parentTask.TotalSubTasks)
                    {
                        parentTask.Status = TaskStatusEnum.Completed;
                        _taskService.Update(parentTask);
                    }
                }

            }
            Log($"Progress: {parentTask.CompletedSubTasks}/{parentTask.TotalSubTasks} subtasks completed for Task {parentTask.Id}");
            Log($"SubTask Completed: {subTask.Id} at {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
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
