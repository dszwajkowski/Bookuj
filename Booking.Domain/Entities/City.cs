using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Domain.Entities
{
    public class City
    {
        public int ID { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [Required, ForeignKey(nameof(Region))]
        public int RegionID { get; set; }

        public virtual Region? Region { get; set; }
        public virtual IEnumerable<Offer>? Offers { get; set; }
        public virtual IEnumerable<Order>? Orders { get; set; }
    }
}
