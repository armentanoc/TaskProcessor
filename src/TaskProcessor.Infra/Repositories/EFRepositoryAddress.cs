using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;
using TaskProcessor.Infra.Context;

namespace TaskProcessor.Infra.Repositories
{
    public class EFRepositoryAddress : EFRepository<Address>, IRepositoryAddress<Address>
    {
        public EFRepositoryAddress(AppDbContext context) : base(context)
        {
            //required by EF
        }

        public Address GetByZipCode(string zipCode)
        {
            throw new NotImplementedException();
        }
    }
}
