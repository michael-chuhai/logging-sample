using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MySecrets.Interfaces;

namespace MySecrets.Middlewares
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserMiddleware> _logger;

        public UserMiddleware(
            RequestDelegate next,
            ILogger<UserMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService, ILogger<UserMiddleware> logger)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userName = context.User?.Identity?.Name;

                _logger.LogDebug("User is authenticated. Loading user for {username}", userName);

                await userService.LoadCurrentUserAsync(userName);
            }

            await _next(context);
        }
    }
}