using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTenant.Core.Entities
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        [Key]
        public string Id { get; set; }

        public string JwtId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; }

        public bool IsInvalidated { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}
