using API.Extensions;
using API.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

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

            var userId = resultContext.HttpContext.User.GetUserId();

            var unitOfWork = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);

            if (user is null) return;

            user.LastActive = DateTimeOffset.UtcNow;

            await unitOfWork.CompleteAsync();
        }
    }
}
