using System.ComponentModel.DataAnnotations;

namespace Booking.Domain.Entities
{
    public class Region
    {
        public int ID { get; set; }
        [Required, MaxLength(255)]
        public string? Name { get; set; }
        [Required]
        public virtual Country? Country { get; set; }
    }
}
