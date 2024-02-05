namespace TaskProcessor.Domain.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);

        IEnumerable<T> GetAll();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
