using API.DTO;
using API.Entities;

namespace API.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<bool> SaveAllAsync();
        void Update(User user);

        Task<MemberDto?> GetMemberByUsernameAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
    }
}
