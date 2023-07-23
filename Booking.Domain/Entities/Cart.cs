using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Domain.Entities
{
    public class Cart
    {
        [MaxLength(32)]
        public string ID { get; set; } = Guid.NewGuid().ToString("N");
        [MaxLength(100)]
        public string? SessionID { get; set; }
        [ForeignKey(nameof(User))]
        public string? UserID { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
