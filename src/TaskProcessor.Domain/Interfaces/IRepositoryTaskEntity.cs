using TaskProcessor.Domain.Model.Enums;

namespace TaskProcessor.Domain.Interfaces
{
    public interface IRepositoryTaskEntity<TaskEntity> : IRepository<TaskEntity>
    {
        TaskEntityStatus GetStatusById(int id);
    }
}
