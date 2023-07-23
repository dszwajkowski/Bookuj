using Booking.Domain.Enums;

namespace Booking.Application.Dtos
{
    public class OfferBriefDto
    {
        public int? ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AuthorId { get; set; }
        public string? Username { get; set; }
        public LodgingType? LodgingType { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CityID { get; set; }
        public string? CityName { get; set; }
        public string? AddressLine { get; set; }
        public string? PostalCode { get; set; }
        public string? ThumbnailPath { get; set; }
        public double Rating { get; set; }
        public int OpinionCount { get; set; }
    }
}
