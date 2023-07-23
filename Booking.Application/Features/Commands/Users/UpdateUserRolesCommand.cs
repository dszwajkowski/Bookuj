using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using Booking.Application.Security;
using MediatR;

namespace Booking.Application.Features.Commands.Users
{
    [Authorize(Roles = "Admin")]
    public class UpdateUserRolesCommand : IRequest
    {
        public string Username { get; set; }
        public IEnumerable<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
    }

    public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand>
    {
        private readonly IUserManagerService _userManagerService;

        public UpdateUserRolesCommandHandler(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        public async Task<Unit> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManagerService.FindByUsername(request.Username);

            if (user == null)
            {
                throw new NotFoundException();
            }

            var roles = await _userManagerService.GetRolesAsync(user);
            var result = await _userManagerService.RemoveFromRolesAsync(user, roles);

            result = await _userManagerService.AddToRolesAsync(user,
                request.UserRoles.Where(r => r.IsInRole).Select(r => r.Name));

            return Unit.Value;
        }
    }
}
