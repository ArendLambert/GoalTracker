using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Abstractions;
using Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWT
{
    public class JWTProvider : IJWTProvider
    {
        private readonly JWTOptions _options;
        public JWTProvider(IOptions<JWTOptions> options)
        {
            _options = options.Value;
        }
        public string GenerateToken(UserModel user)
        {
            Claim[] claim = [new("idUser", user.Id.ToString()), new("email", user.Email.ToString())];

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            //var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("10101010101010101010101010101010")),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claim,
                signingCredentials: signingCredentials,
                issuer: _options.Issuer
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
