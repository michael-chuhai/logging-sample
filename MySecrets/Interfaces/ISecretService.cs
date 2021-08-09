using System.Collections.Generic;
using System.Threading.Tasks;
using MySecrets.Data.Entities;

namespace MySecrets.Interfaces
{
    public interface ISecretService
    {
        Task<IReadOnlyCollection<Secret>> GetAsync();

        Task<Secret> GetAsync(int id);

        Task UpdateAsync(Secret secret);

        Task CreateAsync(Secret secret);

        Task DeleteAsync(int id);
    }
}