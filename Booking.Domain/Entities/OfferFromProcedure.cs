using Booking.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Domain.Entities
{
    public class OfferFromStoredProcedure
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
        public string? Path { get; set; }
        public string? FileName { get; set; }
        public double? Rating { get; set; }
        public int OpinionCount { get; set; }
        [NotMapped]
        public string? FullPath => System.IO.Path.Combine(Path, FileName);
    }
}
