using Core.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ThemeRepository : IThemeReposirory
    {
        public AppDbContext Context {  get; }

        public ThemeRepository(AppDbContext context)
        {
            Context = context;
        }

        public async Task AddAsync(ThemeModel entity)
        {
            await Context.Themes.AddAsync(new Theme 
            {
                Id = entity.Id,
                Name = entity.Name,
                PrimaryColor = entity.PrimaryColor,
                SecondaryColor = entity.SecondaryColor,
                AccentColor = entity.AccentColor,
                BackgroundColor = entity.BackgroundColor,
                TextColor = entity.TextColor,
                BorderColor = entity.BorderColor,
                ShadowColor = entity.ShadowColor,
                CardBackground = entity.CardBackground,
                ButtonColor = entity.ButtonColor,
                ButtonTextColor = entity.ButtonTextColor
            });
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Context.Themes.Where(x => x.Id == id).ExecuteDeleteAsync();
            await Context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await Context.Themes.FindAsync(id) != null;
        }

        public async Task<Guid?> ExistsAsDuplicateAsync(ThemeModel entity)
        {
            Theme? theme = await Context.Themes.FirstOrDefaultAsync(x =>
                x.Name == entity.Name &&
                x.PrimaryColor == entity.PrimaryColor &&
                x.SecondaryColor == entity.SecondaryColor &&
                x.AccentColor == entity.AccentColor &&
                x.BackgroundColor == entity.BackgroundColor &&
                x.TextColor == entity.TextColor &&
                x.BorderColor == entity.BorderColor &&
                x.ShadowColor == entity.ShadowColor &&
                x.CardBackground == entity.CardBackground &&
                x.ButtonColor == entity.ButtonColor &&
                x.ButtonTextColor == entity.ButtonTextColor
            );
            if (theme == null)
            {
                return null;
            }
            return theme.Id;
        }

        public async Task<ICollection<ThemeModel>> GetAllAsync()
        {
            return await Context.Themes.AsNoTracking()
                .Select(x => new ThemeModel(x.Id, x.Name, x.PrimaryColor, x.SecondaryColor, x.AccentColor, 
                x.BackgroundColor, x.TextColor, x.BorderColor, x.ShadowColor, x.CardBackground, x.ButtonColor, x.ButtonTextColor)).ToListAsync();
        }

        public async Task<ThemeModel?> GetByIdAsync(Guid id)
        {
            Theme? theme = await Context.Themes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (theme == null)
            {
                return null;
            }
            return new ThemeModel(theme.Id, theme.Name, theme.PrimaryColor, theme.SecondaryColor, theme.AccentColor, 
                theme.BackgroundColor, theme.TextColor, theme.BorderColor, theme.ShadowColor, theme.CardBackground, 
                theme.ButtonColor, theme.ButtonTextColor);
        }

        public async Task UpdateAsync(ThemeModel entity)
        {
            Theme? theme = await Context.Themes.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (theme == null)
            {
                throw new InvalidOperationException("Entity not found");
            }
            theme.Name = entity.Name;
            theme.PrimaryColor = entity.PrimaryColor;
            theme.SecondaryColor = entity.SecondaryColor;
            theme.AccentColor = entity.AccentColor;
            theme.BackgroundColor = entity.BackgroundColor;
            theme.TextColor = entity.TextColor;
            theme.BorderColor = entity.BorderColor;
            theme.ShadowColor = entity.ShadowColor;
            theme.CardBackground = entity.CardBackground;
            theme.ButtonColor = entity.ButtonColor;
            theme.ButtonTextColor = entity.ButtonTextColor;
            await Context.SaveChangesAsync();
        }
    }
}
