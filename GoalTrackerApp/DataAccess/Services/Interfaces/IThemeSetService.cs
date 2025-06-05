using Core.Models;

namespace DataAccess.Services.Interfaces
{
    public interface IThemeSetService
    {
        Task<Guid> AddAsync(Guid idTheme, Guid? idUserCreator, bool @public);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<ICollection<ThemeSetModel>> GetAllAsync();
        Task<ThemeSetModel?> GetByIdAsync(Guid id);
        Task<ICollection<ThemeSetModel>> GetAllPublicAsync(Guid idUser);
        Task UpdateAsync(ThemeSetModel entity);
    }
}