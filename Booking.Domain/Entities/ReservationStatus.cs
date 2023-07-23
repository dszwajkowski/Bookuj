using System.ComponentModel.DataAnnotations;

namespace Booking.Domain.Entities
{
    public class ReservationStatus
    {
        public int ID { get; set; }
        [Required, MaxLength(30)]
        public string? Name { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
    }
}
