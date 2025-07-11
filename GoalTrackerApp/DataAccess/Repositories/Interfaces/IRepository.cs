﻿using DataAccess.Context;

namespace DataAccess.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        AppDbContext Context { get; }
        Task<ICollection<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
