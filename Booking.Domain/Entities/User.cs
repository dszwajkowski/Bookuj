using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Booking.Domain.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? SecondName { get; set; }
        [ForeignKey(nameof(City))]
        public int? CityID { get; set; }
        [MaxLength(255)]
        public string? AddressLine { get; set; }
        [MaxLength(20)]
        public string? PostalCode { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public DateTime? LockUntil { get; set; }
        [ForeignKey(nameof(Avatar))]
        public int? AvatarID { get; set; }


        public City City { get; set; }
        public Cart Cart { get; set; }
        public UserAvatar Avatar { get; set; }
        public IEnumerable<OfferOpinion> OfferOpinions { get; set; }
        public IEnumerable<Order> Orders { get; set; }

        public bool IsBlocked => LockUntil is not null && LockUntil > DateTime.Now;
    }
}
