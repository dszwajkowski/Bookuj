using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Booking.Domain.Entities
{
    public class OfferPhoto : File<int>
    {
        [ForeignKey(nameof(Offer))]
        public int OfferID { get; set; }

        [Required]
        public virtual Offer Offer { get; set; }
    }
}
