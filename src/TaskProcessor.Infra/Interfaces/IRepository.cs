namespace TaskProcessor.Infra.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
    }
}
