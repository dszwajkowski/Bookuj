using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Reservations;
using Booking.Application.Features.Queries.Reservations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.WebUI.Pages.Offer.My
{
    public class ReservationsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public PaginatedList<ReservationDto>? Reservations { get; set; }
        public int ReservationID { get; set; }
        public string? StatusMessage { get; set; }

        public ReservationsModel(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1, int pageSize = 1)
        {
            Reservations = await _mediator.Send(new GetReservationsWithPaginationQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                UserID = _currentUser.ID
            });

            return Page();
        }

        public async Task OnPostUpdateFilters(int pageNumber, int pageSize)
        {
            await OnGetAsync(pageNumber, pageSize);
        }

        public async Task<IActionResult> OnPostCancelReservationAsync(int reservationID, int pageNumber, int pageSize)
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
                return await OnGetAsync();
            }
            catch (Exception)
            {
                StatusMessage = $"B��d: Wyst�pi� b��d podczas anulowania rezerwacji {reservationID}. Je�li problem b�dzie dalej wyst�powa� skontaktuj si� z administracj�.";
                return await OnGetAsync();
            }
           
            StatusMessage = $"Anulowano rezerwacj� {reservationID}.";

            // TODO: Wys�anie maila do klienta, powiadomienie?

            return await OnGetAsync();
        }
    }
}
