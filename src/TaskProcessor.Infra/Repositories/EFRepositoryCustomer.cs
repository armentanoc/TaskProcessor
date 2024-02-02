using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;
using TaskProcessor.Infra.Context;

namespace TaskProcessor.Infra.Repositories
{
    public class EFRepositoryCustomer : EFRepository<Customer>, IRepositoryCustomer<Customer>
    {
        public EFRepositoryCustomer(AppDbContext context) : base(context)
        {
            //required by EF
        }

        public Customer GetByName(string name)
        {
            return GetAll().FirstOrDefault(c => c.Name == name);
        }
    }
}
