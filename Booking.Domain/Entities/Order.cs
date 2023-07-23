using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Booking.Domain.Entities
{
    public class Order
    {
        [MaxLength(32)]
        public string ID { get; private set; } = Guid.NewGuid().ToString("N");
        [Required]
        public bool IsFinalized { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime? DateFinalized { get; set; }
        [ForeignKey(nameof(PaymentMethod))]
        public int PaymentMethodID { get; set; }
        [MaxLength(450), ForeignKey(nameof(User))]
        public string? UserID { get; set; }
        [Required, MaxLength(256)]
        public string? Email { get; set; }
        [Required, MaxLength(20)]
        public string? PhoneNumber { get; set; }
        [Required, MaxLength(50)]
        public string? FirstName { get; set; }
        [Required, MaxLength(50)]
        public string? SecondName { get; set; }
        [Required, ForeignKey(nameof(City))]
        public int? CityID { get; set; }
        [Required, MaxLength(255)]
        public string? AddressLine { get; set; }
        [Required, MaxLength(20)]
        public string? PostalCode { get; set; }

        public virtual User User { get; set; }
        public virtual City City { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual IEnumerable<Reservation> Reservations { get; set; }
    }
}
