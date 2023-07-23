namespace Booking.Application.Common.Models
{
    public class OfferFilters
    {
        public int? City { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? PersonCount { get; set; }
        public int? RoomCount { get; set; }
        public int? BedCount { get; set; }
        public int? PriceMin { get; set; }
        public int? PriceMax { get; set; }
        public double? SizeMin { get; set; }
        public double? SizeMax { get; set; }
        public string? AuthorID { get; set; }
    }
}
