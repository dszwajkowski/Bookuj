using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Commands.Reservations
{
    public  class UpdateReservationCommand : IRequest
    {
        public int ID { get; set; }
        public int StatusID { get; set; }
    }

    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand>
    {
        private readonly IApplicationDataContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<User> _userManager;
        private readonly IUserManagerService _userManagerService;

        public UpdateReservationCommandHandler(IApplicationDataContext context,
            ICurrentUserService currentUser, UserManager<User> userManager, IUserManagerService userManagerService)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
            _userManagerService = userManagerService;
        }

        public async Task<Unit> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            Reservation reservation;

            try
            {
                reservation = await _context.Reservation
                    .Where(r => r.ID == request.ID)
                    .Include("LodgingOption.Offer")
                    .Include("Cart")
                    .Include("Order")
                    .SingleAsync();
            }
            catch (Exception e)
            {
                // TODO : log
                throw;
            }

            // TODO : const, ex
            if (request.StatusID == 4 && reservation.DateFrom > DateTime.UtcNow.AddDays(2))
            {
                throw new Exception();
            }

            var userID = _currentUser.ID;
            var user = await _userManager.FindByIdAsync(userID);

            if (reservation.LodgingOption?.Offer?.AuthorId != userID
                && (reservation.Cart.UserID != userID || reservation.Order.UserID != userID)
                && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                throw new NotAuthorizedException();
            }

            reservation.StatusID = request.StatusID;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                // TODO: log
                throw;
            }

            return Unit.Value;
        }
    }
}
