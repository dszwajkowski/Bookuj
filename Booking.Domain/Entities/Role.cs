using Microsoft.AspNetCore.Identity;

namespace Booking.Domain.Entities
{
    public class Role : IdentityRole
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
