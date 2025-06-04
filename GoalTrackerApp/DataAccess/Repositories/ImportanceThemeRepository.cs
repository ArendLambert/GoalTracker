using Core.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ImportanceThemeRepository : IImportanceThemeRepository
    {
        public AppDbContext Context { get; }

        public ImportanceThemeRepository(AppDbContext context)
        {
            Context = context;
        }

        public async Task AddAsync(ImportanceThemeModel entity)
        {
            await Context.ImportanceThemes.AddAsync(new ImportanceTheme
            {
                Id = entity.Id,
                IdTheme = entity.IdTheme,
                IdImportance = entity.IdImportance,
                TextColor = entity.TextColor,
                BackgroundColor = entity.BackgroundColor,
            });
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Context.ImportanceThemes.Where(x => x.Id == id).ExecuteDeleteAsync();
            await Context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await Context.ImportanceThemes.FindAsync(id) != null;
        }

        public async Task<ICollection<ImportanceThemeModel>> GetAllAsync()
        {
            return await Context.ImportanceThemes.AsNoTracking()
                .Select(x => new ImportanceThemeModel(x.Id, x.IdImportance, x.IdTheme, x.BackgroundColor, x.TextColor))
                .ToListAsync();
        }

        public async Task<ImportanceThemeModel?> GetByIdAsync(Guid id)
        {
            ImportanceTheme? importanceTheme = await Context.ImportanceThemes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (importanceTheme == null)
            {
                return null;
            }
            return new ImportanceThemeModel(importanceTheme.Id, importanceTheme.IdImportance, importanceTheme.IdTheme,
                importanceTheme.BackgroundColor, importanceTheme.TextColor);
        }

        public async Task<ICollection<ImportanceThemeModel>> GetByThemeIdAsync(Guid id)
        {
            return await Context.ImportanceThemes.Where(x => x.IdTheme == id)
                .Select(x => new ImportanceThemeModel(x.Id, x.IdImportance, x.IdTheme, x.BackgroundColor, x.TextColor))
                .ToListAsync();
        }

        public async Task UpdateAsync(ImportanceThemeModel entity)
        {
            ImportanceTheme? importanceTheme = await Context.ImportanceThemes.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (importanceTheme == null)
            {
                throw new InvalidOperationException("Entity not found");
            }
            importanceTheme.IdImportance = entity.IdImportance;
            importanceTheme.IdTheme = entity.IdTheme;
            importanceTheme.BackgroundColor = entity.BackgroundColor;
            importanceTheme.TextColor = entity.TextColor;
            await Context.SaveChangesAsync();
        }
    }
}
