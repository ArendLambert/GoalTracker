using Core.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.Services.Interfaces;

namespace DataAccess.Services
{
    public class GoalEmailService : IGoalEmailService
    {
        private readonly IGoalEmailRepository _goalEmailRepository;
        public GoalEmailService(IGoalEmailRepository goalEmailRepository)
        {
            _goalEmailRepository = goalEmailRepository;
        }

        public async Task<Guid> AddAsync(Guid idSendEmail, Guid idGoal)
        {
            Guid id = Guid.NewGuid();
            await _goalEmailRepository.AddAsync(new GoalEmailModel(id, idSendEmail, idGoal));
            return id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _goalEmailRepository.DeleteAsync(id);
            return !(await _goalEmailRepository.ExistsAsync(id));
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _goalEmailRepository.ExistsAsync(id);
        }

        public async Task<ICollection<GoalEmailModel>> GetAllAsync()
        {
            return await _goalEmailRepository.GetAllAsync();
        }

        public Task<GoalEmailModel?> GetByIdAsync(Guid id)
        {
            return _goalEmailRepository.GetByIdAsync(id);
        }

        public async Task<ICollection<GoalEmailModel>> GetByIdSendEmailAsync(Guid id)
        {
            return await _goalEmailRepository.GetByIdSendEmailAsync(id);
        }

        public async Task<ICollection<GoalEmailModel>> GetByIdGoalAsync(Guid id)
        {
            return await _goalEmailRepository.GetByIdGoalAsync(id);
        }

        public async Task UpdateAsync(GoalEmailModel entity)
        {
            await _goalEmailRepository.UpdateAsync(entity);
        }
    }
}
