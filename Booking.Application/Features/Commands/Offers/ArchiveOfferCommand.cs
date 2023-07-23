using AutoMapper;
using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Commands.Offers
{
    public class ArchiveOfferCommand : IRequest
    {
        public int ID { get; set; }
        public bool Archived { get; set; }
    }

    public class ArchiveOfferCommandHandler : IRequestHandler<ArchiveOfferCommand>
    {
        private readonly IApplicationDataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUser;

        public ArchiveOfferCommandHandler(IApplicationDataContext context, UserManager<User> userManager, ICurrentUserService currentUser)
        {
            _context = context;
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(ArchiveOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _context.Offer
                .Where(o => o.ID == request.ID)
                .SingleAsync();

            if (offer == null) throw new NotFoundException();
            
            var currentUser = await _userManager.FindByIdAsync(_currentUser.ID);
            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (offer.AuthorId != _currentUser.ID && !isAdmin)
            {
                throw new NotAuthorizedException();
            }

            offer.Archived = request.Archived;            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
        