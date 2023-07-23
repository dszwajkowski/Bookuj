using Booking.Application.Common.Interfaces;
using Booking.Application.Features.Commands.Offers;
using Booking.Application.Features.Queries.Dictionaries;
using Bookuj.WebUI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Booking.WebUI.Pages.Offer
{
    [Authorize]
    public class Create : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        [BindProperty]
        public CreateOfferViewModel Offer { get; set; } = new CreateOfferViewModel();
        [BindProperty]
        public CreateOfferCommand Command { get; set; } = new CreateOfferCommand();
        public SelectList? Cities { get; set; }
        [BindProperty]
        public int[]? FacilitiesIDs { get; set; }
        public SelectList? Facilities { get; set; }
        [TempData]
        public string? StatusMessage { get; set; }

        public Create(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task OnGetAsync()
        {
            var cities = await _mediator.Send(new GetCitiesQuery());
            Cities = new SelectList(cities, "ID", "Name");
            var facilities = await _mediator.Send(new GetFacilitiesQuery());
            Facilities = new SelectList(facilities, "ID", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int? result = null;
            try
            {
                result = await _mediator.Send(new CreateOfferCommand
                {
                    Title = Offer.Title,
                    Description = Offer.Description,
                    AddressLine = Offer.AddressLine,
                    CityID = Offer.CityID,
                    PostalCode = Offer.PostalCode,
                    Photos = Offer.Photos
                });
            }
            catch (Exception e)
            {
                StatusMessage = "B³¹d: " + e.Message;
            }

            if (result is not null)
            {
                return RedirectToPage("/Offer/Details", new { id = result });
            }

            return Page();
        }
    }
}
