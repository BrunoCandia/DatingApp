using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class UserLikeController : ControllerBase
    {
        private readonly ILikeUserRepository _likeUserRepository;
        private readonly IUserRepository _userRepository;

        public UserLikeController(ILikeUserRepository likeUserRepository, IUserRepository userRepository)
        {
            _likeUserRepository = likeUserRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{targetUserId:Guid}")]
        public async Task<ActionResult> ToogleLike(Guid targetUserId)
        {
            var sourceUserId = User.GetUserId();

            if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself");

            var existingUserLike = await _likeUserRepository.GetUserLikeAsync(sourceUserId, targetUserId);

            if (existingUserLike is null)
            {
                var userLike = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId,
                    SourceUser = await _userRepository.GetUserByIdAsync(sourceUserId) ?? throw new ArgumentException("sourceUserId not found"),  // Maybe required can be removed
                    TargetUser = await _userRepository.GetUserByIdAsync(targetUserId) ?? throw new ArgumentException("targetUserId not found")  // Maybe required can be removed
                };

                await _likeUserRepository.AddUserLikeAsync(userLike);
            }
            else
            {
                _likeUserRepository.DeleteUserLike(existingUserLike);
            }

            if (await _likeUserRepository.SaveAllAsync())
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to like user");
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Guid>>> GetCurrentUserLikeIds()
        {
            var userId = User.GetUserId();
            var userLikeIds = await _likeUserRepository.GetCurrentUserLikesIdsAsync(userId);
            return Ok(userLikeIds);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery] LikeParams likeParams)
        {
            likeParams.UserId = User.GetUserId();

            var userLikes = await _likeUserRepository.GetUserLikesAsync(likeParams);

            Response.AddPaginationHeader(userLikes);

            return Ok(userLikes);
        }

        ////[HttpGet]
        ////public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes(string predicate)
        ////{
        ////    var userId = User.GetUserId();
        ////    var userLikes = await _likeUserRepository.GetUserLikesAsync(predicate, userId);
        ////    return Ok(userLikes);
        ////}
    }
}
