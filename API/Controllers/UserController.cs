using API.DTO;
using API.Extensions;
using API.Helpers;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]     //// http://localhost:5000/api/user
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]   //// http://localhost:5000/api/user
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            userParams.CurrentUserName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users);

            return Ok(users);
        }

        ////[HttpGet]   //// http://localhost:5000/api/user
        ////public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        ////{
        ////    var users = await _userRepository.GetMembersAsync();

        ////    if (users is null || !users.Any())
        ////    {
        ////        return NotFound();
        ////    }

        ////    return Ok(users);
        ////}

        //[Authorize(Roles = "Member")]
        [HttpGet("{userName}")]   //// http://localhost:5000/api/user/{userName}
        public async Task<ActionResult<MemberDto>> GetUser(string userName)
        {
            var user = await _userRepository.GetMemberByUsernameAsync(userName);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut()]   //// http://localhost:5000/api/user/
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userName is null)
            {
                return BadRequest("User Name not found in token");
            }

            var user = await _userRepository.GetUserByUsernameAsync(userName);

            if (user is null)
            {
                return NotFound("User not found");
            }

            user.Introduction = memberUpdateDto.Introdcution;
            user.LookingFor = memberUpdateDto.LookingFor;
            user.Interests = memberUpdateDto.Interests;
            user.City = memberUpdateDto.City;
            user.Country = memberUpdateDto.Country;

            ////_userRepository.Update(user);

            if (await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to update user");
        }

        ////[HttpGet]   //// http://localhost:5000/api/user
        ////public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        ////{
        ////    var users = await _userRepository.GetUsersAsync();

        ////    if (users is null || !users.Any())
        ////    {
        ////        return NotFound();
        ////    }

        ////    var usersToReturn = users.Select(x => new MemberDto
        ////    {
        ////        UserId = x.Id,
        ////        UserName = x.UserName,
        ////        KnownAs = x.KnownAs,
        ////        CreatedAt = x.CreatedAt,
        ////        LastActive = x.LastActive,
        ////        Gender = x.Gender,
        ////        Introduction = x.Introduction,
        ////        Interests = x.Interests,
        ////        LookingFor = x.LookingFor,
        ////        City = x.City,
        ////        Country = x.Country,
        ////        Age = x.GetAge(),
        ////        PhotoUrl = x.Photos.FirstOrDefault(x => x.IsMain)?.Url,
        ////        Photos = x.Photos.Select(p => new PhotoDto
        ////        {
        ////            PhotoId = p.PhotoId,
        ////            Url = p.Url,
        ////            IsMain = p.IsMain
        ////        }).ToList()
        ////    }).ToList();

        ////    return Ok(usersToReturn);
        ////}

        ////[HttpGet("{userId}")]   //// http://localhost:5000/api/user/{userId}
        ////public async Task<ActionResult<MemberDto>> GetUser(Guid userId)
        ////{
        ////    var user = await _userRepository.GetUserByIdAsync(userId);

        ////    if (user is null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    var userToReturn = new MemberDto
        ////    {
        ////        UserId = user.Id,
        ////        UserName = user.UserName,
        ////        KnownAs = user.KnownAs,
        ////        CreatedAt = user.CreatedAt,
        ////        LastActive = user.LastActive,
        ////        Gender = user.Gender,
        ////        Introduction = user.Introduction,
        ////        Interests = user.Interests,
        ////        LookingFor = user.LookingFor,
        ////        City = user.City,
        ////        Country = user.Country,
        ////        Age = user.GetAge(),
        ////        PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
        ////        Photos = user.Photos.Select(p => new PhotoDto
        ////        {
        ////            PhotoId = p.PhotoId,
        ////            Url = p.Url,
        ////            IsMain = p.IsMain
        ////        }).ToList()
        ////    };

        ////    return Ok(userToReturn);
        ////}
    }
}
