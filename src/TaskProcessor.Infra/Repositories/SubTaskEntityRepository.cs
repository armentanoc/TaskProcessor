
using TaskProcessor.Domain.Model;
using TaskProcessor.Infra.Context;
using TaskProcessor.Infra.Interfaces;

namespace TaskProcessor.Infra.Repositories
{
    public class SubTaskEntityRepository : Repository<SubTaskEntity>, ISubTaskEntityRepository
    {
        public SubTaskEntityRepository(AppDbContext context) : base(context)
        {
            //required by EF
        }
        public TaskEntity GetByTaskId(int id)
        {
           return GetByTaskId(id);
        }
    }
}
