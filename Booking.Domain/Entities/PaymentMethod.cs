using System.ComponentModel.DataAnnotations;

namespace Booking.Domain.Entities
{
    public class PaymentMethod
    {
        public int ID { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; }
    }
}