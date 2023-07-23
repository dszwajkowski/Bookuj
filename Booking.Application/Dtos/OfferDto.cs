using Booking.Domain.Enums;

namespace Booking.Application.Dtos
{
    public class OfferDto
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AuthorID { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? AvatarPath { get; set; }
        public LodgingType LodgingType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CityID { get; set; }
        public string? CityName { get; set; }
        public string? AddressLine { get; set; }
        public string? PostalCode { get; set; }
        public double Rating { get; set; }
        public int OpinionCount { get; set; }
        public ICollection<OfferPhotoDto>? OfferPhotos { get; set; }
    }
}
