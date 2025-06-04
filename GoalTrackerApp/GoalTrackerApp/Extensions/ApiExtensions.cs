using System.Text;
using JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GoalTrackerApp.Extensions
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentifications(this IServiceCollection services, IConfiguration configuration)
        {

            JWTOptions? jwtOptions = configuration.GetSection("JWTOptions").Get<JWTOptions>();
            if (jwtOptions == null)
            {
                throw new ArgumentNullException(nameof(jwtOptions), "JWT options are not configured in appsettings.json");
            }
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            //Console.WriteLine(context.Request.Cookies["goida"]);
                            context.Token = context.Request.Cookies["goida"];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }
    }
}
