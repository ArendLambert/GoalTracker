using Core.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.Services.Interfaces;

namespace DataAccess.Services
{
    public class ThemeSetService : IThemeSetService
    {
        private readonly IThemeSetRepository _themeSetRepository;
        public ThemeSetService(IThemeSetRepository themeSetRepository)
        {
            _themeSetRepository = themeSetRepository;
        }

        public async Task AddAsync(Guid idTheme, Guid? idUserCreator, bool @public)
        {
            await _themeSetRepository.AddAsync(new ThemeSetModel(Guid.NewGuid(), idTheme, idUserCreator, @public));
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _themeSetRepository.DeleteAsync(id);
            return !(await _themeSetRepository.ExistsAsync(id));
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _themeSetRepository.ExistsAsync(id);
        }

        public async Task<ICollection<ThemeSetModel>> GetAllAsync()
        {
            return await _themeSetRepository.GetAllAsync();
        }

        public async Task<ICollection<ThemeSetModel>> GetAllPublicAsync()
        {
            return await _themeSetRepository.GetAllPublicAsync();
        }

        public async Task<ThemeSetModel?> GetByIdAsync(Guid id)
        {
            return await _themeSetRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(ThemeSetModel entity)
        {
            await _themeSetRepository.UpdateAsync(entity);
        }
    }
}
