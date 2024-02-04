
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Domain.Interfaces
{
    public interface IRepositoryTaskEntity<TaskEntity> : IRepository<TaskEntity>
    {
        TaskStatusEnum GetStatusById(int id);
    }
}
