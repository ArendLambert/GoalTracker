using Core.Models;

namespace DataAccess.Services.Interfaces
{
    public interface IImportanceThemeService
    {
        Task AddAsync(Guid idImportance, Guid idTheme, string backgroundColor, string textColor);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<ICollection<ImportanceThemeModel>> GetAllAsync();
        Task<ImportanceThemeModel?> GetByIdAsync(Guid id);
        Task UpdateAsync(ImportanceThemeModel entity);
        Task<ICollection<ImportanceThemeModel>> GetByThemeIdAsync(Guid id);
    }
}