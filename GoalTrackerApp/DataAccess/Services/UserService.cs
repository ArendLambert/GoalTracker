using Abstractions;
using Core.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.Services.Interfaces;

namespace DataAccess.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJWTProvider _jwtProvider;
        public UserService(IUserRepository userRepository, IJWTProvider jWTProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jWTProvider;
        }

        public async Task AddAsync(string email, string password)
        {
            await _userRepository.AddAsync(new UserModel(Guid.NewGuid(), email, PasswordHasher.PasswordHasher.Generate(password), 
                Guid.Parse("5EC6627F-1F1B-47E6-8EBD-367BC345F702")));
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
            return !(await _userRepository.ExistsAsync(id));
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _userRepository.ExistsAsync(id);
        }

        public async Task<ICollection<UserModel>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<UserModel?> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(UserModel userModel)
        {
            await _userRepository.UpdateAsync(userModel);
        }

        public async Task<UserModel?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (!PasswordHasher.PasswordHasher.Verify(password, user.Password))
            {
                throw new Exception("Incorrect password");
            }
            return _jwtProvider.GenerateToken(user);
        }
    }
}
