using Core.Models;

namespace Abstractions
{
    public interface IJWTProvider
    {
        string GenerateToken(UserModel user);
    }
}