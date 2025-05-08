using API.DTO;
using API.Entities;
using API.Helpers;

namespace API.Repositories
{
    public interface ILikeUserRepository
    {
        Task<UserLike?> GetUserLikeAsync(Guid sourceUserId, Guid targetUserId);
        Task<PagedList<MemberDto>> GetUserLikesAsync(LikeParams likeParams);
        Task<IEnumerable<MemberDto>> GetUserLikesAsync(string predicate, Guid userId);
        Task<IEnumerable<Guid>> GetCurrentUserLikesIdsAsync(Guid currentUserId);
        void DeleteUserLike(UserLike userLike);
        Task AddUserLikeAsync(UserLike userLike);
    }
}
