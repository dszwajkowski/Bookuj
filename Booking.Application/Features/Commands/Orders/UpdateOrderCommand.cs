using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using MediatR;

namespace Booking.Application.Features.Commands.Orders
{
    public class UpdateOrderCommand : IRequest
    {
        public string OrderID { get; set; }
        public bool IsFinalized { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IApplicationDataContext _context;

        public UpdateOrderCommandHandler(IApplicationDataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Order
                .FindAsync(new object[] { request.OrderID }, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException();
            }

            order.IsFinalized = request.IsFinalized;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
