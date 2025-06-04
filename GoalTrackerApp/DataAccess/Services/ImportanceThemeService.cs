using Core.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.Services.Interfaces;

namespace DataAccess.Services
{
    public class ImportanceThemeService : IImportanceThemeService
    {
        private readonly IImportanceThemeRepository _importanceThemeRepository;
        public ImportanceThemeService(IImportanceThemeRepository importanceThemeRepository)
        {
            _importanceThemeRepository = importanceThemeRepository;
        }

        public async Task AddAsync(Guid idImportance, Guid idTheme, string backgroundColor, string textColor)
        {
            await _importanceThemeRepository.AddAsync(new ImportanceThemeModel(Guid.NewGuid(), idImportance,
                idTheme, backgroundColor, textColor));
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _importanceThemeRepository.DeleteAsync(id);
            return !(await _importanceThemeRepository.ExistsAsync(id));
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _importanceThemeRepository.ExistsAsync(id);
        }

        public async Task<ICollection<ImportanceThemeModel>> GetAllAsync()
        {
            return await _importanceThemeRepository.GetAllAsync();
        }

        public async Task<ImportanceThemeModel?> GetByIdAsync(Guid id)
        {
            return await _importanceThemeRepository.GetByIdAsync(id);
        }

        public async Task<ICollection<ImportanceThemeModel>> GetByThemeIdAsync(Guid id)
        {
            return await _importanceThemeRepository.GetByThemeIdAsync(id);
        }

        public async Task UpdateAsync(ImportanceThemeModel entity)
        {
            await _importanceThemeRepository.UpdateAsync(entity);
        }
    }
}
