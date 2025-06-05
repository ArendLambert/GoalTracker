using Core.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ThemeSetRepository : IThemeSetRepository
    {
        public AppDbContext Context { get; }
        public ThemeSetRepository(AppDbContext context)
        {
            Context = context;
        }

        public async Task AddAsync(ThemeSetModel entity)
        {
            await Context.ThemeSets.AddAsync(new ThemeSet
            {
                Id = entity.Id,
                IdTheme = entity.IdTheme,
                Public = entity.Public,
                UserCreator = entity.IdUserCreator
            });
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Context.ThemeSets.Where(x => x.Id == id).ExecuteDeleteAsync();
            await Context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await Context.ThemeSets.FindAsync(id) != null;
        }

        public async Task<ICollection<ThemeSetModel>> GetAllAsync()
        {
            return await Context.ThemeSets.AsNoTracking()
                .Select(x => new ThemeSetModel(x.Id, x.IdTheme, x.UserCreator, x.Public))
                .ToListAsync();
        }

        public async Task<ICollection<ThemeSetModel>> GetAllPublicAsync(Guid idUser)
        {
            return await Context.ThemeSets.AsNoTracking()
                .Where(x => x.Public || x.UserCreator == idUser)
                .Select(x => new ThemeSetModel(x.Id, x.IdTheme, x.UserCreator, x.Public))
                .ToListAsync();
        }

        public async Task<ThemeSetModel?> GetByIdAsync(Guid id)
        {
            ThemeSet? themeSet = await Context.ThemeSets.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (themeSet == null)
            {
                return null;
            }
            return new ThemeSetModel(themeSet.Id, themeSet.IdTheme, themeSet.UserCreator, themeSet.Public);
        }

        public async Task UpdateAsync(ThemeSetModel entity)
        {
            ThemeSet? themeSet = await Context.ThemeSets.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (themeSet == null)
            {
                throw new InvalidOperationException("Entity not found");
            }
            themeSet.IdTheme = entity.IdTheme;
            themeSet.UserCreator = entity.IdUserCreator;
            themeSet.Public = entity.Public;
            await Context.SaveChangesAsync();
        }
    }
}
