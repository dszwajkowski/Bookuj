using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.OfferOpinions;
using Booking.Application.Features.Queries.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.WebUI.Pages.Order
{
    public class MyModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public PaginatedList<OrderDto>? Orders { get; set; }
        [BindProperty]
        public int OfferID { get; set; }    
        [BindProperty]
        public int OpinionRating { get; set; }
        [BindProperty]
        public string OpinionText { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public MyModel(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 25)
        {
            Orders = await _mediator.Send(new GetOrdersWithPaginationQuery
            {
                UserID = _currentUser.ID,
                PageNumber = pageNumber,
                PageSize = 10
            });
        }

        public async Task OnPostAddOpinion(int offerID, int opinionRating, string opinionText)
        {
            var result = await _mediator.Send(new CreateOfferOpinionCommand
            {
                OfferID = offerID,
                UserID = _currentUser.ID,
                Rating = opinionRating,
                Text = opinionText
            });

            StatusMessage = "Pomyœlnie dodano opinie.";

            await OnGetAsync();
        }

        public async Task OnPostUpdateFiltersAsync(int pageNumber, int pageSize)
        {
            await OnGetAsync(pageNumber, pageSize);
        }
    }
}
