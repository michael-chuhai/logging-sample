using Microsoft.AspNetCore.Identity;

namespace MySecrets.Data.Entities
{
    public class Secret : EntityBase
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }
    }
}