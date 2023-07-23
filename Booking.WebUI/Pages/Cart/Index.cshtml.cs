using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Carts;
using Booking.Application.Features.Commands.Orders;
using Booking.Application.Features.Commands.Reservations;
using Booking.Application.Features.Queries.Carts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.WebUI.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<Booking.Domain.Entities.User> _userManager;
        private readonly ICurrentUserService _currentUser;

        public CartDto? Cart { get; set; }
        public string StatusMessage { get; set; }

        public IndexModel(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                bool isAuthenticated = User.Identity is not null && User.Identity.IsAuthenticated;

                Cart = isAuthenticated
                    ? await _mediator.Send(new GetUserCartQuery { UserID = _currentUser.ID })
                    : await _mediator.Send(new GetSessionCartQuery { SessionID = HttpContext.Session.Id });
            }
            catch (InvalidOperationException)
            {
                var result = await _mediator.Send(new CreateCartCommand { SessionID = HttpContext.Session.Id });
                Cart = await _mediator.Send(new GetCartQuery { ID = result });
            }
            // save anything to session to make sure session id stays the same between requests
            HttpContext.Session.SetString("CartID", Cart.ID);

            return Page();
        }

        public async Task OnPostDeleteFromCartAsync(int reservationID)
        {
            await _mediator.Send(new DeleteReservationCommand { ID = reservationID });
        }
    }
}
