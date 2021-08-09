using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MySecrets.Data.Entities
{
    public class Secret : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }
    }
}