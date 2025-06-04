using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using DataAccess.Context;
using DataAccess.Repositories.Interfaces;
using DataAccess.Services.Interfaces;

namespace DataAccess.Services
{
    public class ImportanceService : IImportanceService
    {
        private readonly IRepository<ImportanceModel> _importanceRepository;
        public ImportanceService(IRepository<ImportanceModel> importanceRepository)
        {
            _importanceRepository = importanceRepository;
        }

        public async Task AddAsync(string title, int minDays, int maxDays)
        {
            if (minDays > maxDays)
            {
                int temp = minDays;
                minDays = maxDays;
                maxDays = temp;
            }
            if (minDays < 0 || maxDays < 0)
            {
                minDays = 0;
                maxDays = 0;
            }
            await _importanceRepository.AddAsync(new ImportanceModel(Guid.NewGuid(), title, minDays, maxDays));
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _importanceRepository.DeleteAsync(id);
            return !(await _importanceRepository.ExistsAsync(id));
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _importanceRepository.ExistsAsync(id);
        }

        public async Task<ICollection<ImportanceModel>> GetAllAsync()
        {
            return await _importanceRepository.GetAllAsync();
        }

        public async Task<ImportanceModel?> GetByIdAsync(Guid id)
        {
            return await _importanceRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(ImportanceModel entity)
        {
            await _importanceRepository.UpdateAsync(entity);
        }
    }
}
