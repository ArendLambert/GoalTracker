using Core.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IThemeReposirory : IRepository<ThemeModel>
    {
        Task<Guid?> ExistsAsDuplicateAsync(ThemeModel entity);
    }
}
