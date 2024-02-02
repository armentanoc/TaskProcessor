namespace TaskProcessor.Domain.Interfaces
{
    public interface IRepositoryAddress<Address> : IRepository<Address>
    {
        Address GetByZipCode(string zipCode);
    }
}
