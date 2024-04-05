
using TaskProcessor.Domain.Model;
using TaskProcessor.Infra.Context;
using TaskProcessor.Infra.Interfaces;

namespace TaskProcessor.Infra.Repositories
{
    public class TaskEntityRepository : Repository<TaskEntity>, ITaskEntityRepository
    {
        public TaskEntityRepository(AppDbContext context) : base(context)
        {
            //required by EF
        }

        public TaskStatusEnum GetStatusById(int id)
        {
            return GetStatusById(id);
        }
    }
}
