using System.ComponentModel.DataAnnotations;

namespace Booking.Domain.Entities
{
    public class Country
    {
        public int ID { get; set; }
        [Required, MaxLength(2)]
        public string? Code { get; set; }
        [Required, MaxLength(255)]
        public string? Name { get; set; }
    }
}
