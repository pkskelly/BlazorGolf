using BlazorGolf.Api.Entities;

namespace BlazorGolf.Api.Services
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(string id);
        Task<T> Add(T entity);
        Task Remove(string id);
    }
}