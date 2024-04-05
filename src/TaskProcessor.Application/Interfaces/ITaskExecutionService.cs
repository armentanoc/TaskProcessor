using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Interfaces
{
    public interface ITaskExecutionService
    {
        Task ExecuteTopTasksWithSubTasksAsync(int topTasksCount, int idOfTaskToPause = -1);
        bool NoMoreTasksToProcess();
        Task ExecuteTaskAsync(TaskEntity task);
        Task ExecuteSubTaskAsync(SubTaskEntity subTask, TaskEntity parentTask);
        bool IsCompleted<T>(T entity);
        void TryUpdatingElapsedTime(SubTaskEntity subTask);
        void TryCompletingMainTask(SubTaskEntity subTask, TaskEntity parentTask);
        IEnumerable<TaskEntity> GetTopTasksByPriority(int topTasksCount);
    }
}
