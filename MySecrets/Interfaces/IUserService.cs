using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MySecrets.Interfaces
{
    public interface IUserService
    {
        IdentityUser CurrentUser { get; }

        Task<bool> LoadCurrentUserAsync(string userName);
    }
}