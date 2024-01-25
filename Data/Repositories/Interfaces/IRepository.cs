﻿using AgendaLarAPI.Models.Base;

namespace AgendaLarAPI.Data.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetPagedAsync(int pageSize, int pageIndex);
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
    }
}
