using AgendaLarAPI.Models.Base;

namespace AgendaLarAPI.Data.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task<T?> GetByIdAsync(string loggedUserId, Guid id);
        Task<List<T>> GetAllAsync(string loggedUserId);
        Task<List<T>> GetPagedAsync(string loggedUserId, int pageSize, int pageIndex);
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
    }
}
