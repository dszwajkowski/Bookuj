using Booking.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Domain.Entities
{
    public class Offer
    {
        public int ID { get; set; }
        [Required]
        public bool Archived { get; set; } = false;
        [Required, ForeignKey(nameof(Author))]
        public string? AuthorId { get; set; }
        [Required, MaxLength(50)]
        public string? Title { get; set; }
        [Required, MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        public LodgingType LodgingType { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        [Required, MaxLength(255)]
        public string? AddressLine { get; set; }
        [Required, MaxLength(20)]
        public string? PostalCode { get; set; }
        [Required, ForeignKey(nameof(City))]
        public int? CityID { get; set; }

        public virtual User? Author { get; set; }
        public virtual City? City { get; set; }
        public virtual ICollection<LodgingOption>? LodgingOptions { get; set; }
        public virtual ICollection<OfferPhoto>? OfferPhotos { get; set; }
        public virtual ICollection<OfferOpinion>? OfferOpinions { get; set; }

    }
}
