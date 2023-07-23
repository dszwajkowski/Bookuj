using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Users;
using Booking.Application.Features.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.WebUI.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public PaginatedList<UserBriefDto>? Users { get; set; }
        //[BindProperty]
        //public string? UserID { get; set; }
        [BindProperty]
        public DateTime? LockUntil { get; set; } = DateTime.UtcNow.AddDays(1);

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync(int? pageNumber, int? pageSize)
        {
            Users = await _mediator.Send(new GetUsersWithPaginationQuery
            {
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 25,
            });
        }

        public async Task OnPostAsync(int? pageNumber, int? pageSize)
        {
            await OnGetAsync(pageNumber, pageSize);
        }

        public async Task OnPostBlockUserAsync(string userID, DateTime? lockUntil, int? pageNumber, int? pageSize)
        {
            await _mediator.Send(new UpdateUserLockDateCommand
            {
                UserID = userID,
                LockUntil = lockUntil
            });
            await OnGetAsync(pageNumber,pageSize);
        }

        public async Task OnPostUpdateFiltersAsync(int? pageNumber, int? pageSize)
        {
            await OnGetAsync(pageNumber, pageSize);
        }
    }
}
