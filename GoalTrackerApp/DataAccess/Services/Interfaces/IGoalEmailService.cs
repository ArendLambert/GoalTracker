using Core.Models;

namespace DataAccess.Services.Interfaces
{
    public interface IGoalEmailService
    {
        Task<Guid> AddAsync(Guid idSendEmail, Guid idGoal);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<ICollection<GoalEmailModel>> GetAllAsync();
        Task<GoalEmailModel?> GetByIdAsync(Guid id);
        Task<ICollection<GoalEmailModel>> GetByIdGoalAsync(Guid id);
        Task<ICollection<GoalEmailModel>> GetByIdSendEmailAsync(Guid id);
        Task UpdateAsync(GoalEmailModel entity);
    }
}