using API.Entities;

namespace API.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user);
    }
}
