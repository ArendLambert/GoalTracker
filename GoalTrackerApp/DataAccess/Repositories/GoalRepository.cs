using Core.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        public AppDbContext Context { get; }
        public GoalRepository(AppDbContext context)
        {
            Context = context;
        }

        public async Task AddAsync(GoalModel entity)
        {
            await Context.AddAsync(new Goal 
            {
                Id = entity.Id,
                IdImportance = entity.IdImportance,
                Title = entity.Title,
                Deadline = entity.Deadline,
                Description = entity.Description,
                IdStatus = entity.IdStatus,
                IdUser = entity.IdUser,
                StartDate = entity.StartDate,
                Punishment = entity.Punishment,
                AutoImportance = entity.AutoImportance,
            });
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Context.Goals.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await Context.Goals.FindAsync(id) != null;
        }

        public async Task<ICollection<GoalModel>> GetAllAsync()
        {
            return await Context.Goals.AsNoTracking()
                .Select(x => new GoalModel(x.Id, x.Title, x.Description, x.IdStatus, x.IdImportance, 
                x.IdUser, x.StartDate, x.Deadline, x.Punishment, x.AutoImportance))
                .ToListAsync();
        }

        public async Task<GoalModel?> GetByIdAsync(Guid id)
        {
            Goal? goal = await Context.Goals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (goal == null)
            {
                return null;
            }
            return new GoalModel(goal.Id, goal.Title, goal.Description, goal.IdStatus, goal.IdImportance,
                goal.IdUser, goal.StartDate, goal.Deadline, goal.Punishment, goal.AutoImportance);
        }

        public async Task<ICollection<GoalModel>> GetByUserIdAsync(Guid id)
        {
            return await Context.Goals.AsNoTracking()
                .Where(x => x.IdUser == id)
                .Select(x => new GoalModel(x.Id, x.Title, x.Description, x.IdStatus, x.IdImportance,
                x.IdUser, x.StartDate, x.Deadline, x.Punishment, x.AutoImportance))
                .ToListAsync();
        }

        public async Task UpdateAsync(GoalModel entity)
        {
            Goal? goal = await Context.Goals.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (goal == null)
            {
                throw new InvalidOperationException("Entity not found");
            }
            goal.Title = entity.Title;
            goal.Description = entity.Description;
            goal.IdStatus = entity.IdStatus;
            goal.IdImportance = entity.IdImportance;
            goal.IdUser = entity.IdUser;
            goal.StartDate = entity.StartDate;
            goal.Deadline = entity.Deadline;
            goal.Punishment = entity.Punishment;
            goal.AutoImportance = entity.AutoImportance;
            await Context.SaveChangesAsync();
        }
    }
}
