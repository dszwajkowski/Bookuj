using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Commands.OfferOpinions
{
    public class DeleteOfferOpinionCommand : IRequest
    {
        public int ID { get; set; }
    }

    public class DeleteOfferOpinionCommandHandler : IRequestHandler<DeleteOfferOpinionCommand>
    {
        private readonly IApplicationDataContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<User> _userManager;

        public DeleteOfferOpinionCommandHandler(IApplicationDataContext context, ICurrentUserService currentUser, UserManager<User> userManager)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteOfferOpinionCommand request, CancellationToken cancellationToken)
        {
            var opinion = await _context.OfferOpinion
                .Where(op => op.ID == request.ID)
                .SingleAsync();

            if (opinion is null)
            {
                throw new NotFoundException();
            }

            var currentUser = await _userManager.FindByIdAsync(_currentUser.ID);
            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (opinion.AuthorID != _currentUser.ID && !isAdmin)
            {
                throw new NotAuthorizedException();
            }

            _context.OfferOpinion.Remove(opinion);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
