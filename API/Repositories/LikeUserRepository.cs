using API.Data;
using API.DTO;
using API.Entities;
using API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class LikeUserRepository : ILikeUserRepository
    {
        private readonly DataContext _dataContext;

        public LikeUserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddUserLikeAsync(UserLike userLike)
        {
            await _dataContext.UserLikes.AddAsync(userLike);
        }

        public void DeleteUserLike(UserLike userLike)
        {
            _dataContext.UserLikes.Remove(userLike);
        }

        public async Task<IEnumerable<Guid>> GetCurrentUserLikesIdsAsync(Guid currentUserId)
        {
            return await _dataContext.UserLikes
                .Where(x => x.SourceUserId == currentUserId)
                .Select(x => x.TargetUserId)
                .ToListAsync();
        }

        public async Task<UserLike?> GetUserLikeAsync(Guid sourceUserId, Guid targetUserId)
        {
            return await _dataContext.UserLikes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<MemberDto>> GetUserLikesAsync(LikeParams paramsLikeParams)
        {
            var query = _dataContext.UserLikes
                .Include(x => x.SourceUser.Photos)
                .Include(x => x.TargetUser.Photos)
                .AsQueryable();

            IQueryable<MemberDto> finalQuery;

            switch (paramsLikeParams.Predicate)
            {
                case "liked":
                    {
                        finalQuery = query.Where(x => x.SourceUserId == paramsLikeParams.UserId)
                            .Select(x => new MemberDto
                            {
                                UserId = x.TargetUser.Id,
                                UserName = x.TargetUser.UserName,
                                KnownAs = x.TargetUser.KnownAs,
                                CreatedAt = x.TargetUser.CreatedAt,
                                LastActive = x.TargetUser.LastActive,
                                Gender = x.TargetUser.Gender,
                                Introduction = x.TargetUser.Introduction,
                                Interests = x.TargetUser.Interests,
                                LookingFor = x.TargetUser.LookingFor,
                                City = x.TargetUser.City,
                                Country = x.TargetUser.Country,
                                PhotoUrl = x.TargetUser.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(),
                                Photos = x.TargetUser.Photos.Select(p => new PhotoDto
                                {
                                    PhotoId = p.PhotoId,
                                    Url = p.Url,
                                    IsMain = p.IsMain
                                }).ToList()
                            });

                        break;
                    }
                case "likedBy":
                    {
                        finalQuery = query.Where(x => x.TargetUserId == paramsLikeParams.UserId)
                            .Select(x => new MemberDto
                            {
                                UserId = x.SourceUser.Id,
                                UserName = x.SourceUser.UserName,
                                KnownAs = x.SourceUser.KnownAs,
                                CreatedAt = x.SourceUser.CreatedAt,
                                LastActive = x.SourceUser.LastActive,
                                Gender = x.SourceUser.Gender,
                                Introduction = x.SourceUser.Introduction,
                                Interests = x.SourceUser.Interests,
                                LookingFor = x.SourceUser.LookingFor,
                                City = x.SourceUser.City,
                                Country = x.SourceUser.Country,
                                PhotoUrl = x.SourceUser.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(),
                                Photos = x.SourceUser.Photos.Select(p => new PhotoDto
                                {
                                    PhotoId = p.PhotoId,
                                    Url = p.Url,
                                    IsMain = p.IsMain
                                }).ToList()
                            });

                        break;
                    }
                default:
                    {
                        var userLikeIds = await GetCurrentUserLikesIdsAsync(paramsLikeParams.UserId);

                        finalQuery = query.Where(x => x.TargetUserId == paramsLikeParams.UserId && userLikeIds.Contains(x.SourceUserId))
                            .Select(x => new MemberDto
                            {
                                UserId = x.SourceUser.Id,
                                UserName = x.SourceUser.UserName,
                                KnownAs = x.SourceUser.KnownAs,
                                CreatedAt = x.SourceUser.CreatedAt,
                                LastActive = x.SourceUser.LastActive,
                                Gender = x.SourceUser.Gender,
                                Introduction = x.SourceUser.Introduction,
                                Interests = x.SourceUser.Interests,
                                LookingFor = x.SourceUser.LookingFor,
                                City = x.SourceUser.City,
                                Country = x.SourceUser.Country,
                                PhotoUrl = x.SourceUser.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(),
                                Photos = x.SourceUser.Photos.Select(p => new PhotoDto
                                {
                                    PhotoId = p.PhotoId,
                                    Url = p.Url,
                                    IsMain = p.IsMain
                                }).ToList()
                            });

                        break;
                    }
            }

            return await PagedList<MemberDto>.CreateAsync(finalQuery, paramsLikeParams.PageNumber, paramsLikeParams.PageSize);
        }

        public async Task<IEnumerable<MemberDto>> GetUserLikesAsync(string predicate, Guid userId)
        {
            var query = _dataContext.UserLikes
                .Include(x => x.SourceUser.Photos)
                .Include(x => x.TargetUser.Photos)
                .AsQueryable();

            switch (predicate)
            {
                case "liked":
                    {
                        return await query.Where(x => x.SourceUserId == userId)
                            .Select(x => new MemberDto
                            {
                                UserId = x.TargetUser.Id,
                                UserName = x.TargetUser.UserName,
                                KnownAs = x.TargetUser.KnownAs,
                                CreatedAt = x.TargetUser.CreatedAt,
                                LastActive = x.TargetUser.LastActive,
                                Gender = x.TargetUser.Gender,
                                Introduction = x.TargetUser.Introduction,
                                Interests = x.TargetUser.Interests,
                                LookingFor = x.TargetUser.LookingFor,
                                City = x.TargetUser.City,
                                Country = x.TargetUser.Country,
                                PhotoUrl = x.TargetUser.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(),
                                Photos = x.TargetUser.Photos.Select(p => new PhotoDto
                                {
                                    PhotoId = p.PhotoId,
                                    Url = p.Url,
                                    IsMain = p.IsMain
                                }).ToList()
                            }).ToListAsync();
                    }
                case "likedBy":
                    {
                        return await query.Where(x => x.TargetUserId == userId)
                            .Select(x => new MemberDto
                            {
                                UserId = x.SourceUser.Id,
                                UserName = x.SourceUser.UserName,
                                KnownAs = x.SourceUser.KnownAs,
                                CreatedAt = x.SourceUser.CreatedAt,
                                LastActive = x.SourceUser.LastActive,
                                Gender = x.SourceUser.Gender,
                                Introduction = x.SourceUser.Introduction,
                                Interests = x.SourceUser.Interests,
                                LookingFor = x.SourceUser.LookingFor,
                                City = x.SourceUser.City,
                                Country = x.SourceUser.Country,
                                PhotoUrl = x.SourceUser.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(),
                                Photos = x.SourceUser.Photos.Select(p => new PhotoDto
                                {
                                    PhotoId = p.PhotoId,
                                    Url = p.Url,
                                    IsMain = p.IsMain
                                }).ToList()
                            }).ToListAsync();
                    }
                default:
                    {
                        var userLikeIds = await GetCurrentUserLikesIdsAsync(userId);

                        return await query.Where(x => x.TargetUserId == userId && userLikeIds.Contains(x.SourceUserId))
                            .Select(x => new MemberDto
                            {
                                UserId = x.SourceUser.Id,
                                UserName = x.SourceUser.UserName,
                                KnownAs = x.SourceUser.KnownAs,
                                CreatedAt = x.SourceUser.CreatedAt,
                                LastActive = x.SourceUser.LastActive,
                                Gender = x.SourceUser.Gender,
                                Introduction = x.SourceUser.Introduction,
                                Interests = x.SourceUser.Interests,
                                LookingFor = x.SourceUser.LookingFor,
                                City = x.SourceUser.City,
                                Country = x.SourceUser.Country,
                                PhotoUrl = x.SourceUser.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(),
                                Photos = x.SourceUser.Photos.Select(p => new PhotoDto
                                {
                                    PhotoId = p.PhotoId,
                                    Url = p.Url,
                                    IsMain = p.IsMain
                                }).ToList()
                            }).ToListAsync();
                    }
            }
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
