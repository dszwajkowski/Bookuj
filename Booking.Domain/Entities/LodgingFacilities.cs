using System.ComponentModel.DataAnnotations;

namespace Booking.Domain.Entities
{
    public class LodgingFacilities
    {
        public int ID { get; set; }
        [Required, MaxLength(50)]
        public string? Name { get; set; }
        public virtual ICollection<LodgingOption>? LodgingOptions { get; set; }
    }
}
