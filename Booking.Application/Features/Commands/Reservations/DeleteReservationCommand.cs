using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Commands.Reservations
{
    public  class DeleteReservationCommand : IRequest
    {
        public int ID { get; set; }
    }

    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand>
    {
        private readonly IApplicationDataContext _context;

        public DeleteReservationCommandHandler(IApplicationDataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservation
                 .FindAsync(new object[] { request.ID }, cancellationToken);

            if (reservation is null)
            {
                throw new NotFoundException();
            }

            _context.Reservation.Remove(reservation);

            // TODO: recalculate sum
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
