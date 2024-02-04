using TaskProcessor.Domain.Model;

namespace TaskProcessor.Domain.Interfaces
{
    public interface IRepositorySubTaskEntity<SubTask> : IRepository<SubTask>
    {
        TaskEntity GetByTaskId(int taskId);
    }
}
