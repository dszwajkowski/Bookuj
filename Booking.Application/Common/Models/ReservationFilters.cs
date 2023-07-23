namespace Booking.Application.Common.Models
{
    public class ReservationFilters
    {
        public int? StatusID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
