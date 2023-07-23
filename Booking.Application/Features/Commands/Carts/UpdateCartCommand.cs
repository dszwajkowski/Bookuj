using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;

namespace Booking.Application.Features.Commands.Carts
{
    public class UpdateCartCommand : IRequest
    {
        public string? CartID { get; set; }
        public ICollection<int> ReservationsIDs { get; set; } = new List<int>();
    }

    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand>
    {
        private readonly IApplicationDataContext _context;

        public UpdateCartCommandHandler(IApplicationDataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            //var reservations = _context.Cart
                //.
            return Unit.Value;
        }
    }
}
