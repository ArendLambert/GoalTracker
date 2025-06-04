using Core.Models;

namespace DataAccess.Services.Interfaces
{
    public interface IImportanceService
    {
        Task AddAsync(string title, int minDays, int maxDays);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<ICollection<ImportanceModel>> GetAllAsync();
        Task<ImportanceModel?> GetByIdAsync(Guid id);
        Task UpdateAsync(ImportanceModel entity);
    }
}