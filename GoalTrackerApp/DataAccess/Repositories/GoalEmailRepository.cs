using Core.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class GoalEmailRepository : IGoalEmailRepository
    {
        public AppDbContext Context { get; }
        public GoalEmailRepository(AppDbContext context)
        {
            Context = context;
        }

        public async Task AddAsync(GoalEmailModel entity)
        {
            await Context.GoalEmails.AddAsync(new GoalEmail 
            {
                Id = entity.Id,
                IdGoal = entity.IdGoal,
                IdSendEmail = entity.IdSendEmail
            });
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Context.GoalEmails.Where(x => x.Id == id).ExecuteDeleteAsync();
            await Context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await Context.GoalEmails.FindAsync(id) != null;
        }

        public async Task<ICollection<GoalEmailModel>> GetAllAsync()
        {
            return await Context.GoalEmails.AsNoTracking().Select(x => new GoalEmailModel(x.Id, x.IdSendEmail, x.IdGoal)).ToArrayAsync();
        }

        public async Task<GoalEmailModel?> GetByIdAsync(Guid id)
        {
            GoalEmail? goalEmail = await Context.GoalEmails.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (goalEmail == null)
            {
                return null;
            }
            return new GoalEmailModel(goalEmail.Id, goalEmail.IdSendEmail, goalEmail.IdGoal);
        }

        public async Task UpdateAsync(GoalEmailModel entity)
        {
            GoalEmail? goalEmail = await Context.GoalEmails.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (goalEmail == null)
            {
                throw new InvalidDataException("Entity not found");
            }
            goalEmail.IdSendEmail = entity.IdSendEmail;
            goalEmail.IdGoal = entity.IdGoal;
            await Context.SaveChangesAsync();
        }

        public async Task<ICollection<GoalEmailModel>> GetByIdSendEmailAsync(Guid id)
        {
            return await Context.GoalEmails.AsNoTracking()
                .Where(x => x.IdSendEmail == id)
                .Select(x => new GoalEmailModel(x.Id, x.IdSendEmail, x.IdGoal))
                .ToListAsync();
        }

        public async Task<ICollection<GoalEmailModel>> GetByIdGoalAsync(Guid id)
        {
            return await Context.GoalEmails.AsNoTracking()
                .Where(x => x.IdGoal == id)
                .Select(x => new GoalEmailModel(x.Id, x.IdSendEmail, x.IdGoal))
                .ToListAsync();
        }
    }
}
