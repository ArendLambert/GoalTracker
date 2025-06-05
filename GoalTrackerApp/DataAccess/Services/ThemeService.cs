using Core.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.Services.Interfaces;

namespace DataAccess.Services
{
    public class ThemeService : IThemeService
    {
        private readonly IThemeReposirory _themeRepository;
        public ThemeService(IThemeReposirory themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public async Task<Guid> AddAsync(string name, string primaryColor, string secondaryColor, string accentColor,
            string backgroundColor, string textColor, string borderColor, string shadowColor, string cardBackground,
            string buttonColor, string buttonTextColor)
        {
            Guid id = Guid.NewGuid();
            await _themeRepository.AddAsync(new ThemeModel(id, name, primaryColor, secondaryColor,
                accentColor, backgroundColor, textColor, borderColor, shadowColor, cardBackground, buttonColor, buttonTextColor));
            return id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _themeRepository.DeleteAsync(id);
            return !(await _themeRepository.ExistsAsync(id));
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _themeRepository.ExistsAsync(id);
        }

        public async Task<Guid?> ExistsAsDuplicateAsync(string name, string primaryColor, string secondaryColor, string accentColor,
            string backgroundColor, string textColor, string borderColor, string shadowColor, string cardBackground,
            string buttonColor, string buttonTextColor)
        {
            return await _themeRepository.ExistsAsDuplicateAsync(new ThemeModel(new Guid(), name, primaryColor, secondaryColor,
                accentColor, backgroundColor, textColor, borderColor, shadowColor, cardBackground, buttonColor, buttonTextColor));
        }

        public async Task<ICollection<ThemeModel>> GetAllAsync()
        {
            return await _themeRepository.GetAllAsync();
        }

        public async Task<ThemeModel?> GetByIdAsync(Guid id)
        {
            return await _themeRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(ThemeModel entity)
        {
            await _themeRepository.UpdateAsync(entity);
        }
    }
}
