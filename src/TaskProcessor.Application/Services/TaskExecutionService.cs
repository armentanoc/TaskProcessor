
using TaskProcessor.Application.Services;

namespace TaskProcessor.Domain.Model
{
    public class TaskExecutionService
    {
        private readonly TaskService _taskService;
        private readonly SubTaskService _subtaskService;
        private readonly object _taskLockObject = new object();
        private readonly object _subtaskLockObject = new object();

        public TaskExecutionService(TaskService taskService, SubTaskService subTaskService)
        {
            _taskService = taskService;
            _subtaskService = subTaskService;
        }

        public async Task ExecuteTopTasksWithSubTasksAsync(int topTasksCount)
        {
            var tasksByPriority = GetTopTasksByPriority(topTasksCount);
            var executionTasks = tasksByPriority.Select(ExecuteTaskAsync);
            await Task.WhenAll(executionTasks).ConfigureAwait(false);
        }

        private async Task ExecuteTaskAsync(TaskEntity task)
        {
            if (!(task.Status == TaskStatusEnum.InProgress))
            {
                //ServiceHelper.Log($"\nStarting Task: {TaskService.GetTaskInformation(task)}");
                //logar essa informação
                AlterStatusToInProgress(_taskLockObject, _taskService, task);
            }
            else
            {
                //ServiceHelper.LogWithColor(ConsoleColor.Magenta, $"\nRestarting Task at {(task.CompletedSubTasks) / (task.TotalSubTasks)}: {TaskService.GetTaskInformation(task)}");
            }

            var subTaskExecutionTasks = GetSubTasks(task)
                .Select(subTask => ExecuteSubTaskAsync(subTask, task))
                .ToList();

            await Task.WhenAll(subTaskExecutionTasks).ConfigureAwait(false);
        }

        private IEnumerable<SubTaskEntity> GetSubTasks(TaskEntity task)
        {
            return task.SubTasks
            .Where(subTask => !ServiceHelper.GetExcludedStatuses().Contains(subTask.Status))
            .OrderByDescending(subTask => subTask.ElapsedTime);
        }

        private string GetDateTime()
        {
            return $"At {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
        }

        private async Task ExecuteSubTaskAsync(SubTaskEntity subTask, TaskEntity parentTask)
        {
            //ServiceHelper.Log($"\nStarting SubTask: {GetSubTaskInformation(subTask)}");
            //logar essa informação
            AlterStatusToInProgress(_subtaskLockObject, _subtaskService, subTask);

            while (true)
            {
                TryUpdatingElapsedTime(subTask, parentTask);
                TryCompletingMainTask(subTask, parentTask);
                if (!ServiceHelper.IsCompleted(subTask))
                    //ServiceHelper.LogWithColor(ConsoleColor.Cyan, $"\nSubTask Updated: {GetSubTaskInformation(subTask)}");
                    await Task.Delay(1000);
                //colocar essa informação em log apenas
                else
                    break;
            }
        }

        private void TryUpdatingElapsedTime(SubTaskEntity subTask, TaskEntity parentTask)
        {
            lock (_subtaskLockObject)
            {
                subTask.UpdateElapsedTime();
                _subtaskService.Update(subTask);

                if (subTask.ElapsedTime >= subTask.Duration)
                {
                    subTask.Status = TaskStatusEnum.Completed;
                    parentTask.CompletedSubTasks++;
                    //ServiceHelper.LogWithColor(ConsoleColor.Green, $"\nSubTask Completed: {GetSubTaskInformation(subTask)}");
                    //logar essa informação
                    //ServiceHelper.LogProgress(parentTask);
                    //logar essa informação
                }
            }
        }

        private void TryCompletingMainTask(SubTaskEntity subTask, TaskEntity parentTask)
        {
            if (!ServiceHelper.IsCompleted(parentTask))
            {
                if (parentTask.CompletedSubTasks == parentTask.TotalSubTasks)
                {
                    parentTask.Status = TaskStatusEnum.Completed;
                    lock (_taskLockObject)
                    {
                        _taskService.Update(parentTask);
                        //ServiceHelper.LogWithColor(ConsoleColor.Green, $"\nTask Completed: {TaskService.GetTaskInformation(parentTask)}");
                        //logar essa informação
                    }
                }
            }
        }

        private object GetSubTaskInformation(SubTaskEntity subTask)
        {
            return
                $" \nTaskId: {subTask.TaskId}" +
                $" SubTaskId: {subTask.Id} {GetDateTime()}" +
                $" Duration: {subTask.Duration}" +
                $" ElapsedTime: {subTask.ElapsedTime}";
        }

        private IEnumerable<TaskEntity> GetTopTasksByPriority(int topTasksCount)
        {
            return _taskService
                .GetAllTasksOrderedByPriority()
                .Take(topTasksCount)
                .ToList();
        }
        internal static void AlterStatusToInProgress(object lockObject, TaskService taskService, TaskEntity task)
        {
            task.Status = TaskStatusEnum.InProgress;
            lock (lockObject)
                taskService.Update(task);
        }

        internal static void AlterStatusToInProgress(object lockObject, SubTaskService subTaskService, SubTaskEntity task)
        {
            task.Status = TaskStatusEnum.InProgress;
            lock (lockObject)
                subTaskService.Update(task);
        }
    }
}
