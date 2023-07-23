using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Offers;
using Booking.Application.Features.Queries.Dictionaries;
using Booking.Application.Features.Queries.Offers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Booking.WebUI.Pages.Offer
{
    public class ListModel : PageModel
    {
        private readonly IMediator _mediator;

        public PaginatedList<OfferBriefDto>? Offers { get; set; }
        [BindProperty]
        public OfferFilters Filters { get; set; } = new OfferFilters();
        public SelectList? Cities { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public ListModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(int pageNumber, int pageSize, [FromQuery] OfferFilters filters)
        {
            Filters = filters;
            var query = new GetOffersWithFiltersQuery()
            {
                PageNumber = pageNumber < 1 ? 1 : pageNumber,
                PageSize = pageSize < 1 ? 5 : pageSize,
                Filters = filters
            };


            Offers = await _mediator.Send(query);

            var cities = await _mediator.Send(new GetCitiesQuery());
            Cities = new SelectList(cities, "ID", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateFiltersAsync(int pageNumber, int pageSize, OfferFilters filters)
        {
            return await OnGetAsync(pageNumber, pageSize, filters);
        }

        public async Task<IActionResult> OnPostDeleteOfferAsync(int id, int pageNumber, int pageSize, OfferFilters filters)
        {
            try
            {
                await _mediator.Send(new ArchiveOfferCommand { ID = id, Archived = true });
            }
            catch (NotAuthorizedException)
            {
                StatusMessage = $"B³¹d: Nie masz uprawnieñ ¿eby zarchiwizowaæ ofertê {id}.";
                return await OnGetAsync(pageNumber, pageSize, filters);
            }
            catch (Exception)
            {
                StatusMessage = $"B³¹d: Wyst¹pi³ b³¹d podczas archiwizacji oferty {id}. Jeœli problem bêdzie dalej wystêpowaæ skontaktuj siê z administracj¹.";
                return await OnGetAsync(pageNumber, pageSize, filters);
            }

            StatusMessage = $"Pomyœlnie zarchiwizowano ofertê numer {id}.";
            return await OnGetAsync(pageNumber, pageSize, filters);
        }
    }

    //[BindProperty(SupportsGet = true)]
    //public DateTime? DateFrom { get; set; }
    //[BindProperty(SupportsGet = true)]
    //public DateTime? DateTo { get; set; }
    //[BindProperty(SupportsGet = true)]
    //public int? City { get; set; }
    //[BindProperty(SupportsGet = true)]
    //public int? PersonCount { get; set; }
    //[BindProperty(SupportsGet = true)]
    //public int? RoomCount { get; set; }
    //[BindProperty(SupportsGet = true)]
    //public int? BedCount { get; set; }
    //[BindProperty(SupportsGet = true)]
    //public int? PriceMin { get; set; }
    //[BindProperty(SupportsGet = true)]
    //public int? PriceMax { get; set; }
    //[BindProperty(SupportsGet = true)]
    //public int? SizeMin { get; set; }
    //[BindProperty(SupportsGet = true)]
    //public int? SizeMax { get; set; }
    //[BindProperty(SupportsGet = true)]
    //public string? Author { get; set; }
}
