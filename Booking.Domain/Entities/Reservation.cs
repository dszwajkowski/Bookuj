using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Booking.Domain.Entities
{
    public class Reservation
    {
        public int ID { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [Required, ForeignKey(nameof(Status))]
        public int StatusID { get; set; }
        [ForeignKey(nameof(LodgingOption))]
        public int LodgingOptionID { get; set; }
        [ForeignKey(nameof(Cart))]
        public string CartID { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual ReservationStatus? Status { get; set; }
        [Required]
        public virtual LodgingOption? LodgingOption { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Order Order { get; set; }
    }
}
