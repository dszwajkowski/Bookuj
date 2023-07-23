using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Domain.Entities
{
    public class LodgingOption
    {   
        public int ID { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int PersonCount { get; set; }
        [Required]
        public int RoomCount { get; set; }
        [Required]
        public int BedCount { get; set; }
        [Required]
        public double Size { get; set; }
        [Required, ForeignKey(nameof(Offer))]
        public int OfferID { get; set; }

        public virtual Offer? Offer { get; set; }
        public virtual ICollection<LodgingFacilities>? LodgingFacilities { get; set; } = new List<LodgingFacilities>();
    }
}
