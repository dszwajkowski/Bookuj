using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Booking.Application.Features.Commands.Carts
{
    public class CreateCartCommand : IRequest<string>
    {
        public string? UserID { get; set; }
        public string? SessionID { get; set; }
    }

    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, string>
    {
        private readonly IApplicationDataContext _context;
        private readonly IUserManagerService _userManagerService;

        public CreateCartCommandHandler(IApplicationDataContext context, IUserManagerService userManager)
        {
            _context = context;
            _userManagerService = userManager;
        }

        public async Task<string> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManagerService.FindByIdAsync(request.UserID);

            var cart = new Cart
            {
                UserID = user?.Id,
                SessionID = request.SessionID
            };

            _context.Cart.Add(cart);
            await _context.SaveChangesAsync(cancellationToken);

            return cart.ID;
        }
    }
}
