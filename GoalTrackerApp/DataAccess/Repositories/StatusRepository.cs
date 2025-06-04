using Core.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class StatusRepository : IRepository<StatusModel>
    {
        public AppDbContext Context { get; }
        public StatusRepository(AppDbContext context)
        {
            Context = context;
        }

        public async Task AddAsync(StatusModel entity)
        {
            await Context.Statuses.AddAsync(new Status
            { 
                Id = entity.Id,
                Title = entity.Title,
            });
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Context.Statuses.Where(x => x.Id == id).ExecuteDeleteAsync();
            await Context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await Context.Statuses.FindAsync(id) != null;
        }

        public async Task<ICollection<StatusModel>> GetAllAsync()
        {
            return await Context.Statuses.AsNoTracking().Select(x => new StatusModel(x.Id, x.Title)).ToListAsync();
        }

        public async Task<StatusModel?> GetByIdAsync(Guid id)
        {
            Status? status = await Context.Statuses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if(status == null)
            {
                return null;
            }
            return new StatusModel(status.Id, status.Title);
        }

        public async Task UpdateAsync(StatusModel entity)
        {
            Status? status = await Context.Statuses.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (status == null)
            {
                throw new InvalidOperationException("Entity not found");
            }
            status.Title = entity.Title;
            await Context.SaveChangesAsync();
        }
    }
}
