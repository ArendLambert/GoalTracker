using Core.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IThemeSetRepository : IRepository<ThemeSetModel>
    {
        Task<ICollection<ThemeSetModel>> GetAllPublicAsync(Guid idUser);
    }
}