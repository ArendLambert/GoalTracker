using Core.Models;

namespace DataAccess.Services.Interfaces
{
    public interface IGoalService
    {
        Task<Guid> AddAsync(string title, string? description, Guid idStatus, Guid idImportance, 
            Guid idUser, DateTime startDate, DateTime? deadline, string? punishment, bool autoImportance);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<ICollection<GoalModel>> GetAllAsync();
        Task<GoalModel?> GetByIdAsync(Guid id);
        Task<ICollection<GoalModel>> GetByUserIdAsync(Guid id);
        Task UpdateAsync(GoalModel entity);
        Task UpdateAutoImportance(GoalModel entity, bool autoImportance);
    }
}