using Core.Interfaces;
using Core.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using DataAccess.Services.Interfaces;

namespace DataAccess.Services
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IDateTimeManager _dateTimeManager;
        public GoalService(IGoalRepository goalRepository, IDateTimeManager dateTimeManager)
        {
            _goalRepository = goalRepository;
            _dateTimeManager = dateTimeManager;
        }

        public async Task<Guid> AddAsync(string title, string? description, Guid idStatus, Guid idImportance,
            Guid idUser, DateTime startDate, DateTime? deadline, string? punishment, bool autoImportance)
        {
            Guid guid = Guid.NewGuid();
            await _goalRepository.AddAsync(new GoalModel(guid, title, description, idStatus,
                idImportance, idUser, _dateTimeManager.RemoveSeconds(startDate), deadline, punishment, autoImportance));
            return guid;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _goalRepository.DeleteAsync(id);
            return !(await _goalRepository.ExistsAsync(id));
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _goalRepository.ExistsAsync(id);
        }

        public async Task<ICollection<GoalModel>> GetAllAsync()
        {
            //ICollection<GoalModel> goals = await _goalRepository.GetAllAsync();
            return await _goalRepository.GetAllAsync();
        }

        public async Task<GoalModel?> GetByIdAsync(Guid id)
        {
            return await _goalRepository.GetByIdAsync(id);
        }

        public async Task<ICollection<GoalModel>> GetByUserIdAsync(Guid id)
        {
            return await _goalRepository.GetByUserIdAsync(id);
        }

        public async Task UpdateAsync(GoalModel entity)
        {
            await _goalRepository.UpdateAsync(entity);
        }

        public async Task UpdateAutoImportance(GoalModel entity, bool autoImportance)
        {
            entity.AutoImportance = autoImportance;
            await _goalRepository.UpdateAsync(entity);
        }

        public async Task<ICollection<Goal>> GetDeadlineAsync()
        {
            return await _goalRepository.GetDeadlineAsync();
        }
    }
}
