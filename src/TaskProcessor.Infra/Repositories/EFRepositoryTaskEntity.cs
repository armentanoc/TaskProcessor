using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;
using TaskProcessor.Domain.Model.Enums;
using TaskProcessor.Infra.Context;

namespace TaskProcessor.Infra.Repositories
{
    public class EFRepositoryTaskEntity : EFRepository<TaskEntity>, IRepositoryTaskEntity<TaskEntity>
    {
        public EFRepositoryTaskEntity(AppDbContext context) : base(context)
        {
            //required by EF
        }
        public TaskEntityStatus GetStatusById(int id)
        {
            return GetStatusById(id);
        }
    }
}
