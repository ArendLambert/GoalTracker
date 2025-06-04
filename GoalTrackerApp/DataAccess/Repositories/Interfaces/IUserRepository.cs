using Core.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<UserModel>
    {
        Task<UserModel?> GetByEmailAsync(string email);
    }
}
