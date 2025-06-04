using Core.Models;

namespace DataAccess.Services.Interfaces
{
    public interface IStatusService
    {
        Task AddAsync(string title);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<ICollection<StatusModel>> GetAllAsync();
        Task<StatusModel?> GetByIdAsync(Guid id);
        Task UpdateAsync(StatusModel entity);
    }
}