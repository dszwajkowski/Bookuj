using Booking.Application.Features.Commands.Offers;
using Booking.Application.Features.Queries.Offers;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers.V1
{
    public class OfferController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OfferController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/offer/{id}")]
        public async Task<IActionResult> GetOffer(int id)
        {
            return Ok(await _mediator.Send(new GetOfferQuery { ID = id }));
        }

        [HttpGet("api/offers")]
        public async Task<IActionResult> GetOffersWithPagination([FromQuery]GetOffersWithFiltersQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPut("api/offer")]
        public async Task<ActionResult<int>> CreateOffer(CreateOfferCommand command, [FromBody]List<LodgingOption> lodgingOptions)
        {
            if(lodgingOptions is not null) command.LodgingOptions = lodgingOptions;
            return await _mediator.Send(command);
        }

        [HttpPut("api/offer/update/{id}")]
        public async Task<Unit> UpdateOffer(UpdateOfferCommand command)
        {
            return await _mediator.Send(command);
        }

        /*[HttpDelete("api/offer/{id}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            return Ok(await _mediator.Send(new DeleteOfferCommand { ID = id }));
        }*/
    }
}
