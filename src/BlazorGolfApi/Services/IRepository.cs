using BlazorGolfApi.Entities;

namespace BlazorGolfApi.Services
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(string id);
        Task<T> Add(T entity);
        Task Remove(string id);
    }
}