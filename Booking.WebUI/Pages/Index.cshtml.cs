using Booking.Application.Common.Models;
using Booking.Application.Features.Queries.Dictionaries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Booking.WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public OfferFilters Filters { get; set; } = new OfferFilters();
        public SelectList? Cities { get; set; }

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync()
        {
            var cities = await _mediator.Send(new GetCitiesQuery());
            Cities = new SelectList(cities, "ID", "Name");
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Offer/List", Filters = Filters);
        }
    }
}