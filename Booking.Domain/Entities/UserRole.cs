using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Domain.Entities
{
    public class UserRole : IdentityUserRole<string>
    {
        //[ForeignKey(nameof(User)), Required, MaxLength(450)]
        //public string UserId { get; set; }
        //[ForeignKey(nameof(Role)), Required, MaxLength(450)]
        //public string RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

    }
}
