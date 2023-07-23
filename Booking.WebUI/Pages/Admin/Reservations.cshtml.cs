using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Reservations;
using Booking.Application.Features.Queries.Reservations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookuj.WebUI.Pages.Admin
{
    [Authorize(Roles = ("Admin"))]
    public class ReservationsModel : PageModel
    {
        private readonly IMediator _mediator;

        public PaginatedList<ReservationDto>? Reservations { get; set; }
        public int ReservationID { get; set; }
        public string? StatusMessage { get; set; }

        public ReservationsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Reservations = await _mediator.Send(new GetReservationsWithPaginationQuery
            {
                PageNumber = 1,
                PageSize = 10,
            });

            return Page();
        }

        public async Task<IActionResult> OnPostCancelReservation(int reservationID)
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
                StatusMessage = $"B³¹d: Nie masz uprawnieñ do anulowania rezerwacji {reservationID}.";
                return await OnGetAsync();
            }
            catch (Exception)
            {
                StatusMessage = $"B³¹d: Wyst¹pi³ b³¹d podczas anulowania rezerwacji {reservationID}.";
                return await OnGetAsync();
            }

            StatusMessage = $"Anulowano rezerwacjê {reservationID}.";

            // TODO: Wys³anie maila do klienta, powiadomienie?

            return Page();
        }
    }
}
