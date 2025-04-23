using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var userName = user.FindFirst(ClaimTypes.Name)?.Value ?? throw new InvalidOperationException("User name claim not found");

            return userName;
        }

        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID claim not found");

            return new Guid(userId);
        }
    }
}
