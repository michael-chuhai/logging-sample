using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MySecrets.Interfaces;

namespace MySecrets.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UserService> _logger;


        public IdentityUser CurrentUser { get; private set; }


        public UserService(
            UserManager<IdentityUser> userManager,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }


        public async Task<bool> LoadCurrentUserAsync(string userName)
        {
            try
            {
                CurrentUser = await _userManager.FindByNameAsync(userName);

                return CurrentUser != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load current user {username} due to error.", userName);

                throw;
            }
        }
    }
}