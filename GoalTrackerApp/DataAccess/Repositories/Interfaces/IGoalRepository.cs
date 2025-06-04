using Core.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IGoalRepository : IRepository<GoalModel>
    {
        Task<ICollection<GoalModel>> GetByUserIdAsync(Guid id);
    }
}
