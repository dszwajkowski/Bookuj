using Booking.Domain.Entities;

namespace Booking.Application.Dtos
{
    public class OrderDto
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public bool IsFinalized { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateFinalized { get; set; }
        //[Required, MaxLength(256)]
        //public string Email { get; set; }
        //[Required, MaxLength(20)]
        //public string PhoneNumber { get; set; }
        //[Required, MaxLength(50)]
        //public string FirstName { get; set; }
        //[Required, MaxLength(50)]
        //public string SecondName { get; set; }
        //[Required, ForeignKey(nameof(City))]
        //public int CityID { get; set; }
        //[Required, MaxLength(255)]
        //public string AddressLine { get; set; }
        //[Required, MaxLength(20)]
        //public string PostalCode { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }               
        public IEnumerable<ReservationDto> Reservations { get; set; }
    }
}
