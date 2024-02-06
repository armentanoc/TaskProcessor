using Microsoft.Extensions.Logging;
using TaskProcessor.Application.Services;

namespace TaskProcessor.Domain.Model
{
    public class TaskExecutionService
    {
        private readonly TaskService _taskService;
        private readonly SubTaskService _subtaskService;
        private readonly object _taskLockObject = new object();
        private readonly object _subtaskLockObject = new object();
        private readonly object _logLockObject = new object();
        private readonly string logFilePath = "log_task_execution_service.txt";
        //private readonly ILogger<TaskExecutionService> _logger;

        public TaskExecutionService(TaskService taskService, SubTaskService subTaskService, ILogger<TaskExecutionService> logger)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _subtaskService = subTaskService ?? throw new ArgumentNullException(nameof(subTaskService));
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
            File.WriteAllText(logFilePath, string.Empty);
        }
        public async Task ExecuteTopTasksWithSubTasksAsync(int topTasksCount)
        {
            var tasksByPriority = GetTopTasksByPriority(topTasksCount);
            var executionTasks = tasksByPriority.Select(ExecuteTaskAsync);
            await Task.WhenAll(executionTasks).ConfigureAwait(false);
        }

        private async Task ExecuteTaskAsync(TaskEntity task)
        {
            if (task.Status != TaskStatusEnum.InProgress)
            {
                LogInformation($"\nStarting Task: {TaskService.GetTaskInformation(task)}");
                AlterStatusToInProgress(task);
            }

            var subTaskExecutionTasks = GetSubTasks(task)
                .Select(subTask => ExecuteSubTaskAsync(subTask, task));

            await Task.WhenAll(subTaskExecutionTasks).ConfigureAwait(false);
        }

        private async Task ExecuteSubTaskAsync(SubTaskEntity subTask, TaskEntity parentTask)
        {
            LogInformation($"\nStarting SubTask: {ServiceHelper.GetSubTaskInformation(subTask)}");
            AlterStatusToInProgress(subTask);

            while (!ServiceHelper.IsCompleted(subTask))
            {
                TryUpdatingElapsedTime(subTask, parentTask);
                TryCompletingMainTask(subTask, parentTask);

                if (!ServiceHelper.IsCompleted(subTask))
                {
                    LogInformation($"\nSubTask Updated: {ServiceHelper.GetSubTaskInformation(subTask)}");
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }

            LogInformation($"\nSubTask Completed: {ServiceHelper.GetSubTaskInformation(subTask)}");
            LogInformation($"Task Progress: {parentTask.CompletedSubTasks}/{parentTask.TotalSubTasks}");
        }

        private IEnumerable<SubTaskEntity> GetSubTasks(TaskEntity task)
        {
            return task.SubTasks
                .Where(subTask => !ServiceHelper.GetExcludedStatuses().Contains(subTask.Status))
                .OrderByDescending(subTask => subTask.ElapsedTime);
        }

        private void TryUpdatingElapsedTime(SubTaskEntity subTask, TaskEntity parentTask)
        {
            subTask.UpdateElapsedTime();

            if (subTask.ElapsedTime >= subTask.Duration)
            {
                subTask.MakeSureElapsedTimeMatchesDuration();
                subTask.SetStatusToComplete();
                UpdateSubTaskInDatabase(subTask);
                LogInformation($"\nSubTask Completed: {ServiceHelper.GetSubTaskInformation(subTask)}");

                parentTask.IncreaseCompletedSubTasks();
                UpdateTaskInDatabase(parentTask);

                LogInformation($"Task Progress: {parentTask.CompletedSubTasks}/{parentTask.TotalSubTasks}");
            }
            else
            {
                UpdateSubTaskInDatabase(subTask);
            }
        }

        private void TryCompletingMainTask(SubTaskEntity subTask, TaskEntity parentTask)
        {
            if (!ServiceHelper.IsCompleted(parentTask))
            {
                if (parentTask.CompletedSubTasks >= parentTask.TotalSubTasks)
                {
                    parentTask.SetStatusToComplete();
                    UpdateTaskInDatabase(parentTask);
                }
            }
        }

        internal void AlterStatusToInProgress(TaskEntity task)
        {
            task.SetStatusToInProgress();
            UpdateTaskInDatabase(task);
        }

        internal void AlterStatusToInProgress(SubTaskEntity subTask)
        {
            subTask.SetStatusToInProgess();
            UpdateSubTaskInDatabase(subTask);
        }

        private IEnumerable<TaskEntity> GetTopTasksByPriority(int topTasksCount)
        {
            return _taskService
                .GetAllTasksOrderedByPriority()
                .Take(topTasksCount)
                .ToList();
        }

        private void UpdateSubTaskInDatabase(SubTaskEntity subTask)
        {
            lock (_subtaskLockObject)
                _subtaskService.Update(subTask);
        }

        private void UpdateTaskInDatabase(TaskEntity task)
        {
            lock (_taskLockObject)
                _taskService.Update(task);
        }

        private void LogInformation(string message)
        {
            lock (_logLockObject)
            {
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(
                        $"{ServiceHelper.GetDateTime()}" +
                        $"{message}" +
                        $"\n\n------------------------------------------------------------------------\n"
                    );
                }
            }
        }
    }
}
