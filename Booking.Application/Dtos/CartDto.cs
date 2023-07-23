using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Dtos
{
    public class CartDto
    {
        public string ID { get; set; }
        public IList<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();
    }
}
