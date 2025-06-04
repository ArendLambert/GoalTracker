using Core.Models;
using DataAccess.Entities;

namespace DataAccess.Repositories.Interfaces
{
    public interface IGoalRepository : IRepository<GoalModel>
    {
        Task<ICollection<GoalModel>> GetByUserIdAsync(Guid id);
        Task<ICollection<Goal>> GetDeadlineAsync();
    }
}
