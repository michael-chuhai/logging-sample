using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySecrets.Data;
using MySecrets.Data.Entities;
using MySecrets.Interfaces;

namespace MySecrets.Implementations
{
    public class SecretService : ISecretService
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SecretService> _logger;


        public SecretService(
            IUserService userService,
            ApplicationDbContext context,
            ILogger<SecretService> logger)
        {
            _userService = userService;
            _context = context;
            _logger = logger;
        }


        public async Task<IReadOnlyCollection<Secret>> GetAsync()
        {
            var currentUserId = _userService.CurrentUser.Id;

            _logger.LogDebug("Loading secrets...");

            var secrets = await _context.Secrets
                .Where(s => s.UserId == currentUserId)
                .ToListAsync();

            _logger.LogDebug("Secrets loaded");

            return secrets;
        }

        public async Task<Secret> GetAsync(int id)
        {
            _logger.LogDebug("Loading secret #{secretId}", id);

            var secret = await _context.Secrets.FindAsync(id);

            _logger.LogDebug("Secret loaded");

            return secret;
        }

        public async Task UpdateAsync(Secret secret)
        {
            _logger.LogDebug("Modifying entity state");

            _context.Attach(secret).State = EntityState.Modified;

            try
            {
                _logger.LogDebug("Trying save changes to database");

                await _context.SaveChangesAsync();

                _logger.LogDebug("The secret is successfully updated.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!SecretExists(secret.Id))
                {
                    _logger.LogDebug(ex, "There was a concurrency exception. Due to the secret isn't found, we're ignoring the exception.");

                    return;
                }

                _logger.LogError(ex, "Failed to update the secret due to concurrency error.");

                throw;
            }
        }

        public async Task<Secret> CreateAsync(Secret secret)
        {
            secret.UserId = _userService.CurrentUser.Id;

            _context.Secrets.Add(secret);
            await _context.SaveChangesAsync();

            return secret;
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogDebug("Looking for the secret #{secretId} in the database.", id);

            var secret = await _context.Secrets
                .FirstOrDefaultAsync(m => m.Id == id);

            if (secret == null)
            {
                _logger.LogDebug("The secret isn't found. Doing nothing.");

                return;
            }

            _logger.LogDebug("Marking the secret as deleted");

            _context.Secrets.Remove(secret);

            _logger.LogDebug("Saving changes to db...");

            await _context.SaveChangesAsync();
        }


        private bool SecretExists(int id)
        {
            return _context.Secrets.Any(e => e.Id == id);
        }
    }
}