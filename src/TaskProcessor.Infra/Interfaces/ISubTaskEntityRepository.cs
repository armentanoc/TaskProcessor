using TaskProcessor.Domain.Model;

namespace TaskProcessor.Infra.Interfaces
{
    public interface ISubTaskEntityRepository : IRepository<SubTaskEntity>
    {
        TaskEntity GetByTaskId(int taskId);
    }
}
