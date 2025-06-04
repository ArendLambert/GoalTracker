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

        public async Task<ICollection<StatusModel>> GetAllAsync()
        {
            ICollection<StatusModel> statuses = await _statusRepository.GetAllAsync();
            ICollection<StatusModel> statusesFiltered = new List<StatusModel>();
            statusesFiltered.Add(statuses.First(x => x.Title == "Новая"));
            statusesFiltered.Add(statuses.First(x => x.Title == "В процессе"));
            statusesFiltered.Add(statuses.First(x => x.Title == "На паузе"));
            statusesFiltered.Add(statuses.First(x => x.Title == "Завершена"));
            statusesFiltered.Add(statuses.First(x => x.Title == "Отменена"));
            statusesFiltered.Add(statuses.First(x => x.Title == "Просрочена"));
            return statusesFiltered;
        }

        public async Task<StatusModel?> GetByIdAsync(Guid id)
        {
            return await _statusRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(StatusModel entity)
        {
            await _statusRepository.UpdateAsync(entity);
        }
    }
}
