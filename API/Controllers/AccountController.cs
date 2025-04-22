using API.DTO;
using API.Entities;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExistsAsync(registerDto.UserName)) return BadRequest("Username is taken");

            var user = new User
            {
                UserName = registerDto.UserName.ToLower(),
                DateOfBirth = DateOnlyHelper.StringToDateOnly(registerDto.DateOfBirth),
                KnownAs = registerDto.KnownAs,
                Gender = registerDto.Gender,
                City = registerDto.City,
                Country = registerDto.Country
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateTokenAsync(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.UserName.ToUpper());

            if (user is null || user.UserName is null) return Unauthorized("Invalid user name");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) return Unauthorized("Invalid password");

            return new UserDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Token = await _tokenService.CreateTokenAsync(user),
                Gender = user.Gender,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };
        }

        private async Task<bool> UserExistsAsync(string userName)
        {
            return await _userManager.Users.AnyAsync(x => x.NormalizedUserName == userName.ToUpper()); // Bob != bob
        }
    }
}
