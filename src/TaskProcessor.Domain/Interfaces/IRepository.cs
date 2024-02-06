using TaskProcessor.Domain.Model;

namespace TaskProcessor.Domain.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);

        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
    }
}
