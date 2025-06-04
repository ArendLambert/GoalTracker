using Core.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {   
        public AppDbContext Context { get;  }
        public UserRepository(AppDbContext appDbContext)
        {
            Context = appDbContext;
        }

        public async Task AddAsync(UserModel entity)
        {
            await Context.Users.AddAsync(new User 
            {
                Id = entity.Id,
                Email = entity.Email,
                Password = entity.Password,
                IdThemeSet = entity.IdThemeSet,
            });
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Context.Users.Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            await Context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await Context.Users.FindAsync(id) != null;
        }

        public async Task<ICollection<UserModel>> GetAllAsync()
        {
            return await Context.Users.AsNoTracking()
                .Select(x => new UserModel(x.Id, x.Email, x.Password, x.IdThemeSet)).ToListAsync();
        }

        public async Task<UserModel?> GetByIdAsync(Guid id)
        {
            User? user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            return new UserModel(user.Id, user.Email, user.Password, user.IdThemeSet);
        }

        public async Task<UserModel?> GetByEmailAsync(string email)
        {
            User? user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            return new UserModel(user.Id, user.Email, user.Password, user.IdThemeSet);
        }

        public async Task UpdateAsync(UserModel entity)
        {
            User? user = await Context.Users.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (user == null)
            {
                throw new InvalidOperationException("Entity not found");
            }
            user.Email = entity.Email;
            user.Password = entity.Password;
            user.IdThemeSet = entity.IdThemeSet;
            await Context.SaveChangesAsync();
        }
    }
}
