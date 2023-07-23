using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.OfferOpinions;
using Booking.Application.Features.Commands.Reservations;
using Booking.Application.Features.Queries.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.WebUI.Pages.Order
{
    public class DetailsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        [BindProperty]
        public string OrderID { get; set; }
        [BindProperty]
        public int OfferID { get; set; }
        [BindProperty]
        public int ReservationID { get; set; }
        [BindProperty]
        public int OpinionRating { get; set; }
        [BindProperty]
        public string OpinionText { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public OrderDto Order { get; set; } = new OrderDto();

        public DetailsModel(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task OnGetAsync(string orderID)
        {
            OrderID = orderID;
            Order = await _mediator.Send(new GetOrderQuery { ID = orderID });
        }

        public async Task OnPostAddOpinionAsync(int offerID, int opinionRating, string opinionText, string orderID)
        {
            var result = await _mediator.Send(new CreateOfferOpinionCommand
            {
                OfferID = offerID,
                UserID = _currentUser.ID,
                Rating = opinionRating,
                Text = opinionText
            });

            StatusMessage = "Pomy�lnie dodano opini�.";

            await OnGetAsync(orderID);
        }

        public async Task OnPostCancelReservationAsync(string orderID, int reservationID)
        {
            try
            {
                await _mediator.Send(new UpdateReservationCommand
                {
                    ID = reservationID,
                    StatusID = 4
                });
            }
            catch (NotAuthorizedException)
            {
                StatusMessage = $"B��d: Nie masz uprawnie� do anulowania rezerwacji {reservationID}.";
            }
            catch (Exception)
            {
                StatusMessage = $"B��d: Wyst�pi� b��d podczas anulowania rezerwacji {reservationID}. Je�li problem b�dzie dalej wyst�powa� skontaktuj si� z administracj�.";
            }

            await OnGetAsync(orderID);
        }
    }
}
