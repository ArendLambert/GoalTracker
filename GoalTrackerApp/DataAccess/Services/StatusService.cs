using Core.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.Services.Interfaces;

namespace DataAccess.Services
{
    public class StatusService : IStatusService
    {
        private readonly IRepository<StatusModel> _statusRepository;
        public StatusService(IRepository<StatusModel> statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task AddAsync(string title)
        {
            await _statusRepository.AddAsync(new StatusModel(Guid.NewGuid(), title));
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _statusRepository.DeleteAsync(id);
            return !(await _statusRepository.ExistsAsync(id));
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _statusRepository.ExistsAsync(id);
        }

        public Task<ICollection<StatusModel>> GetAllAsync()
        {
            return _statusRepository.GetAllAsync();
        }

        public Task<StatusModel?> GetByIdAsync(Guid id)
        {
            return _statusRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(StatusModel entity)
        {
            await _statusRepository.UpdateAsync(entity);
        }
    }
}
