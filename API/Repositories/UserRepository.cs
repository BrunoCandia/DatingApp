using API.Data;
using API.DTO;
using API.Entities;
using API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .Include(x => x.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(User user)
        {
            _context.Update(user);
        }

        public async Task<MemberDto?> GetMemberByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(x => x.Photos)
                .Select(x => new MemberDto
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    KnownAs = x.KnownAs,
                    CreatedAt = x.CreatedAt,
                    LastActive = x.LastActive,
                    Gender = x.Gender,
                    Introduction = x.Introduction,
                    Interests = x.Interests,
                    LookingFor = x.LookingFor,
                    City = x.City,
                    Country = x.Country,
                    ////Age = x.GetAge(),
                    ////PhotoUrl = x.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                    ////PhotoUrl = x.Photos.FirstOrDefault(p => p.IsMain) != null ? x.Photos.FirstOrDefault(p => p.IsMain)!.Url : null, // Alternative 1
                    PhotoUrl = x.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(), // Alternative 2
                    Photos = x.Photos.Select(p => new PhotoDto
                    {
                        PhotoId = p.PhotoId,
                        Url = p.Url,
                        IsMain = p.IsMain
                    }).ToList()
                })
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
                .Include(x => x.Photos)
                .Select(x => new MemberDto
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    KnownAs = x.KnownAs,
                    CreatedAt = x.CreatedAt,
                    LastActive = x.LastActive,
                    Gender = x.Gender,
                    Introduction = x.Introduction,
                    Interests = x.Interests,
                    LookingFor = x.LookingFor,
                    City = x.City,
                    Country = x.Country,
                    ////Age = x.GetAge(),
                    ////PhotoUrl = x.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                    ////PhotoUrl = x.Photos.FirstOrDefault(p => p.IsMain) != null ? x.Photos.FirstOrDefault(p => p.IsMain)!.Url : null, // Alternative 1
                    PhotoUrl = x.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(), // Alternative 2
                    Photos = x.Photos.Select(p => new PhotoDto
                    {
                        PhotoId = p.PhotoId,
                        Url = p.Url,
                        IsMain = p.IsMain
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            ////var query = _context.Users
            ////    .Include(x => x.Photos)
            ////    .Select(x => new MemberDto
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
            ////        ////Age = x.GetAge(),
            ////        ////PhotoUrl = x.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            ////        ////PhotoUrl = x.Photos.FirstOrDefault(p => p.IsMain) != null ? x.Photos.FirstOrDefault(p => p.IsMain)!.Url : null, // Alternative 1
            ////        PhotoUrl = x.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(), // Alternative 2
            ////        Photos = x.Photos.Select(p => new PhotoDto
            ////        {
            ////            PhotoId = p.PhotoId,
            ////            Url = p.Url,
            ////            IsMain = p.IsMain
            ////        }).ToList()
            ////    });

            var query = _context.Users
                .Include(x => x.Photos)
                .AsQueryable();

            query = query.Where(x => x.UserName != userParams.CurrentUserName);

            if (userParams.Gender is not null)
            {
                query = query.Where(x => x.Gender == userParams.Gender);
            }

            var minDateOfBirth = DateOnly.FromDateTime(DateTime.Today).AddYears(-userParams.MaxAge - 1);
            var maxDateOfBirth = DateOnly.FromDateTime(DateTime.Today).AddYears(-userParams.MinAge);

            query = query.Where(x => x.DateOfBirth >= minDateOfBirth && x.DateOfBirth <= maxDateOfBirth);

            query = userParams.OrderBy switch
            {
                "createdAt" => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderByDescending(x => x.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(
                query.Select(x => new MemberDto
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    KnownAs = x.KnownAs,
                    CreatedAt = x.CreatedAt,
                    LastActive = x.LastActive,
                    Gender = x.Gender,
                    Introduction = x.Introduction,
                    Interests = x.Interests,
                    LookingFor = x.LookingFor,
                    City = x.City,
                    Country = x.Country,
                    PhotoUrl = x.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(), // Alternative 2
                    Photos = x.Photos.Select(p => new PhotoDto
                    {
                        PhotoId = p.PhotoId,
                        Url = p.Url,
                        IsMain = p.IsMain
                    }).ToList()
                }),
                userParams.PageNumber,
                userParams.PageSize);
        }
    }
}
