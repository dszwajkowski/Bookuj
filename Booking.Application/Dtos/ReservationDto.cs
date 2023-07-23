using Booking.Domain.Entities;

namespace Booking.Application.Dtos
{
    public class ReservationDto
    {
        public int ID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal TotalPrice { get; set; }
        public ReservationStatusDto? Status { get; set; }
        public OfferBriefDto? Offer { get; set; }
        public LodgingOptionDto? LodgingOption { get; set; }
    }
}
