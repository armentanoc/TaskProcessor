
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
            Log($"\n({task.Priority}) Executing Task: {task.Id} with subtasks: {string.Join(", ", task.SubTasks.Select(subTask => $"SubTask Id: {subTask.Id}, Duration: {subTask.Duration.TotalSeconds}s"))}");
            var subTaskExecutionTasks = task.SubTasks.Select(subTask => ExecuteSubTaskAsync(subTask, task)).ToList();
            await Task.WhenAll(subTaskExecutionTasks);
            Log($"Task Completed: {task.Id}");
        }

        private async Task ExecuteSubTaskAsync(SubTaskEntity subTask, TaskEntity parentTask)
        {
            Log($"\nExecuting SubTask: {subTask.Id}" +
                $" Duration: {subTask.Duration}" +
                $" ElapsedTime: {subTask.ElapsedTime}");

            subTask.StartSubTask();

            while (subTask.Status != TaskStatusEnum.Completed)
            {
                Task.Delay(1000); // Wait for 1 second
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
            Log($"SubTask Completed: {subTask.Id}");
        }

        private void Log(string message)
        {
            // Replace with your logging mechanism (e.g., use a logging library)
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
