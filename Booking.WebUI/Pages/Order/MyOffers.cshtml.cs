using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Features.Queries.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.WebUI.Pages.Order
{
    public class MyOffersModel : PageModel
    {
        private readonly IMediator _mediator;

        public PaginatedList<OrderDto>? Orders { get; set; }

        public MyOffersModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 25)
        {
            Orders = await _mediator.Send(new GetOrdersWithPaginationQuery
            {
                PageNumber = pageNumber,
                PageSize = 1
            });
        }
    }
}
