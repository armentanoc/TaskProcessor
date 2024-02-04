using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;
using TaskProcessor.Infra.Context;

namespace TaskProcessor.Infra.Repositories
{
    public class EFRepositorySubTaskEntity : EFRepository<SubTaskEntity>, IRepositorySubTaskEntity<SubTaskEntity>
    {
        public EFRepositorySubTaskEntity(AppDbContext context) : base(context)
        {
            //required by EF
        }
        public TaskEntity GetByTaskId(int id)
        {
           return GetByTaskId(id);
        }
    }
}
