using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddEntityFrameworkStores<DataContext>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var tokenKey = configuration["TokenKey"] ?? throw new InvalidOperationException("TokenKey not found");
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = key
                    };
                });

            // When a user is authenticated via a JWT, the claims embedded in the token are extracted and used to create a ClaimsPrincipal.
            // This ClaimsPrincipal represents the authenticated user and is accessible throughout the application.

            return services;
        }
    }
}
