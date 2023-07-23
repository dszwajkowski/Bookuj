using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Domain.Entities
{
    public class OfferOpinion
    {
        public int ID { get; set; }
        [Required, ForeignKey(nameof(User))]
        public string AuthorID { get; set; }
        [Required, ForeignKey(nameof(Offer))]
        public int OfferID { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required, MaxLength(500)]
        public string Text { get; set; }
        [Required]
        public int Rating { get; set; }

        public virtual User User { get; set; }
        public virtual Offer Offer { get; set; }
    }
}
