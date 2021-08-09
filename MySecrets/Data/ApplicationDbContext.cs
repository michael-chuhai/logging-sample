using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySecrets.Data.Entities;

namespace MySecrets.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Secret> Secrets { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}
