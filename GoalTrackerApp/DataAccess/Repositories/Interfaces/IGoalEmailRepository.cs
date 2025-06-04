using Core.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IGoalEmailRepository : IRepository<GoalEmailModel>
    {
        Task<ICollection<GoalEmailModel>> GetByIdSendEmailAsync(Guid id);
        Task<ICollection<GoalEmailModel>> GetByIdGoalAsync(Guid id);
    }
}
