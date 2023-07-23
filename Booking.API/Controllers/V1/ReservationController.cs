using Booking.Application.Features.Commands.Reservations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers.V1
{
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("api/reservation")]
        public async Task<ActionResult<int>> CreateReservation(CreateReservationCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("api/reservation/update/{id}")]
        public async Task<Unit> UpdateOffer(UpdateReservationCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("api/reservation/{id}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            return Ok(await _mediator.Send(new DeleteReservationCommand { ID = id }));
        }
    }
}
