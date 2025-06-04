using Core.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ImportanceModelrepository : IRepository<ImportanceModel>
    {
        public AppDbContext Context {  get; }

        public ImportanceModelrepository(AppDbContext context)
        {
            Context = context;
        }

        public async Task AddAsync(ImportanceModel entity)
        {
            await Context.Importances.AddAsync(new Importance 
            {
                Id = entity.Id,
                MaxDays = entity.MaxDays,
                MinDays = entity.MinDays,
                Title = entity.Title,
            });
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Context.Importances.Where(x => x.Id == id).ExecuteDeleteAsync();
            await Context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await Context.Importances.FindAsync(id) != null;
        }

        public async Task<ICollection<ImportanceModel>> GetAllAsync()
        {
            return await Context.Importances.AsNoTracking().Select(x => new ImportanceModel(x.Id, x.Title, x.MinDays, x.MaxDays)).ToListAsync();
        }

        public async Task<ImportanceModel?> GetByIdAsync(Guid id)
        {
            Importance? importance = await Context.Importances.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (importance == null)
            {
                return null;
            }
            return new ImportanceModel(importance.Id, importance.Title, importance.MinDays, importance.MaxDays);
        }

        public async Task UpdateAsync(ImportanceModel entity)
        {
            Importance? importance = await Context.Importances.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (importance == null)
            {
                throw new InvalidOperationException("Entity not found");
            }
            importance.Title = entity.Title;
            importance.MinDays = entity.MinDays;
            importance.MaxDays = entity.MaxDays;
            await Context.SaveChangesAsync();
        }
    }
}
