using Core.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IImportanceThemeRepository : IRepository<ImportanceThemeModel>
    {
        Task<ICollection<ImportanceThemeModel>> GetByThemeIdAsync(Guid id);
    }
}