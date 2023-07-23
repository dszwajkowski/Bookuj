using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using Booking.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Commands.Reservations
{
    public  class CreateReservationCommand : IRequest<int>
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int LodgingOptionID { get; set; }
        public string CartID { get; set; }
    }

    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, int>
    {
        private readonly IApplicationDataContext _context;

        public CreateReservationCommandHandler(IApplicationDataContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var lodgingOption = await _context.LodgingOption
                .FindAsync(new object[] { request.LodgingOptionID }, cancellationToken );
            var cart = await _context.Cart
                .Where(c => c.ID == request.CartID)
                .SingleAsync(cancellationToken);

            if (lodgingOption is null || cart is null)
            {
                throw new NotFoundException();
            }

            decimal price = (decimal)(request.DateTo.Date - request.DateFrom.Date).TotalDays * (decimal)lodgingOption.Price;
            var reservation = new Reservation
            {
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                LodgingOptionID = lodgingOption.ID,
                CartID = cart.ID,
                TotalPrice = price,
                StatusID = (int)ReservationStatuses.NotConfirmed
            };

            _context.Reservation.Add(reservation);

            await _context.SaveChangesAsync(cancellationToken);

            return reservation.ID;
        }
    }
}
