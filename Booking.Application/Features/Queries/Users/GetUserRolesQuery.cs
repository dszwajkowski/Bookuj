using Booking.Application.Dtos;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.Users
{
    public class GetUserRolesQuery : IRequest<IList<UserRoleDto>>
    {
        public string Username { get; set; }
    }

    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IList<UserRoleDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetUserRolesQueryHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IList<UserRoleDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var allRoles = await _roleManager.Roles.ToListAsync(cancellationToken);
            List<UserRoleDto> userRoles = new();
            
            foreach (var r in allRoles)
            {
                if (await _userManager.IsInRoleAsync(user, r.Name))
                {
                    userRoles.Add(new UserRoleDto { Name = r.Name, IsInRole = true });
                }
                else
                {
                    userRoles.Add(new UserRoleDto { Name = r.Name, IsInRole = false });
                }
            }

            return userRoles;
        }
    }
}
