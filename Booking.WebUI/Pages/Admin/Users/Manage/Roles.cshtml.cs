using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Users;
using Booking.Application.Features.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.WebUI.Pages.Admin.Users.Manage
{
    public class RolesModel : PageModel
    {
        private readonly IMediator _mediator;

        public string Username { get; set; }
        [BindProperty]
        public IList<UserRoleDto> UserRoles { get; set; }

        public RolesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync(string username)
        {
            Username = username;
            UserRoles = await _mediator.Send(new GetUserRolesQuery { Username = username });
        }

        public async Task OnPostAsync(string username)
        {
            await _mediator.Send(new UpdateUserRolesCommand
            {
                Username = username,
                UserRoles = UserRoles
            });
            await OnGetAsync(username);
        }
    }
}
