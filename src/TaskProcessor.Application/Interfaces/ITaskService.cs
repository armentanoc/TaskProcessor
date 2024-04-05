
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskEntity> GetAllTasks();
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
        IEnumerable<TaskEntity> GetAllTasksByPriorityAndNumberOfSubTasks();
        void CreateTask();
        void CreateSubTasks(TaskEntity task);   
        void Update(TaskEntity task);
        TaskEntity GetTaskById(int taskToPauseId);
    }
}
