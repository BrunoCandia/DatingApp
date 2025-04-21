using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> CreateTokenAsync(User user)
        {
            var tokenKey = _configuration["TokenKey"] ?? throw new InvalidOperationException("Cannot access token key from settings");

            if (tokenKey.Length < 64)
            {
                throw new InvalidOperationException("Token key must be at least 64 characters long");
            }

            // Create a new symmetric security key using the token key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new InvalidOperationException("User name cannot be null");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
