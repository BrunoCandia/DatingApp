using API.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Execute action before the action method is called

            var resultContext = await next();

            // Execute action after the action method is called

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userName = resultContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var userRepository = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

            var user = await userRepository.GetUserByUsernameAsync(userName);

            if (user is null) return;

            user.LastActive = DateTimeOffset.UtcNow;

            await userRepository.SaveAllAsync();
        }
    }
}
