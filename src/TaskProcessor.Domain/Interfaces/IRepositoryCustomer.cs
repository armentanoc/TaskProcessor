using TaskProcessor.Domain.Model;

namespace TaskProcessor.Domain.Interfaces
{
    public interface IRepositoryCustomer<Customer> : IRepository<Customer>
    {
        Customer GetByName(string name);
    }
}
