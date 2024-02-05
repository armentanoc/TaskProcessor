
namespace TaskProcessor.Application.Interfaces
{
    public interface IService<T> where T : class
    {
        public void Update(T entity);
    }
}
