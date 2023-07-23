using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Security;
using MediatR;

namespace Booking.Application.Features.Commands.Users
{
    [Authorize(Roles = "Admin")]
    public class UpdateUserLockDateCommand : IRequest
    {
        public string? UserID { get; set; }
        public DateTime? LockUntil { get; set; }
    }

    public class UpdateUserLockDateCommandHandler : IRequestHandler<UpdateUserLockDateCommand>
    {
        private readonly IApplicationDataContext _context;
        private readonly ICurrentUserService _currentUser;

        public UpdateUserLockDateCommandHandler(IApplicationDataContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdateUserLockDateCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FindAsync(new object[] { request.UserID });

            if (user is null)
            {
                throw new NotFoundException();
            }
            if (user.Id == _currentUser.ID)
            {
                throw new InvalidOperationException("You can't perform this operation on yourself.");
            }

            user.LockUntil = request.LockUntil;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
