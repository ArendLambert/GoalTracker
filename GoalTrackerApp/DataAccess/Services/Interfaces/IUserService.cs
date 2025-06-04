using Core.Models;

namespace DataAccess.Services.Interfaces
{
    public interface IUserService
    {
        Task AddAsync(string email, string password);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<ICollection<UserModel>> GetAllAsync();
        Task<UserModel?> GetByEmailAsync(string email);
        Task<UserModel?> GetByIdAsync(Guid id);
        Task UpdateAsync(UserModel userModel);
        Task<string> Login(string email, string password);
    }
}