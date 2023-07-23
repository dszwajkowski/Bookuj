

using Booking.Domain.Entities;

namespace Booking.Application.Dtos
{
    public class LodgingOptionDto
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public int PersonCount { get; set; }
        public int RoomCount { get; set; }
        public int BedCount { get; set; }
        public double Size { get; set; }
        public ICollection<LodgingFacilitiesDto>? LodgingFacilities { get; set; }
    }
}
