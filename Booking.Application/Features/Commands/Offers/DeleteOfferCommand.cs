using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Booking.Application.Features.Commands.Offers
{
    public class DeleteOfferCommand : IRequest
    {
        public int ID { get; set; }
    }

    public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand>
    {
        private readonly IApplicationDataContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<User> _userManager;

        public DeleteOfferCommandHandler(IApplicationDataContext context, ICurrentUserService currentUser, UserManager<User> userManager)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _context.Offer
                .FindAsync(new object[] { request.ID }, cancellationToken);

            if (offer is null)
            {
                throw new NotFoundException();
            }

            var currentUser = await _userManager.FindByIdAsync(_currentUser.ID);
            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (offer.AuthorId != _currentUser.ID && !isAdmin)
            {
                throw new NotAuthorizedException();
            }

            _context.Offer.Remove(offer);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
