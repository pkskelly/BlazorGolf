namespace BlazorGolf.Api.Services
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(string id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task Delete(string id);
    }
}