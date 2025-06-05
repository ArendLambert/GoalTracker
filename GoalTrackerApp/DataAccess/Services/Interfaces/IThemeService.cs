using Core.Models;

namespace DataAccess.Services.Interfaces
{
    public interface IThemeService
    {
        Task<Guid> AddAsync(string name, string primaryColor, string secondaryColor, string accentColor, string backgroundColor, string textColor, string borderColor, string shadowColor, string cardBackground, string buttonColor, string buttonTextColor);
        Task<bool> DeleteAsync(Guid id);
        Task<Guid?> ExistsAsDuplicateAsync(string name, string primaryColor, string secondaryColor, string accentColor, string backgroundColor, string textColor, string borderColor, string shadowColor, string cardBackground, string buttonColor, string buttonTextColor);
        Task<bool> ExistsAsync(Guid id);
        Task<ICollection<ThemeModel>> GetAllAsync();
        Task<ThemeModel?> GetByIdAsync(Guid id);
        Task UpdateAsync(ThemeModel entity);
    }
}