using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Offers;
using Booking.Application.Features.Queries.Dictionaries;
using Booking.Application.Features.Queries.LodgingOptions;
using Booking.Application.Features.Queries.Offers;
using Bookuj.WebUI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Booking.WebUI.Pages.Offer
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        [BindProperty]
        public EditOfferViewModel Offer { get; set; }
        //public OfferDto Offer { get; set; } = new OfferDto(); 
        [BindProperty]
        public IList<LodgingOptionDto> LodgingOptions { get; set; } = new List<LodgingOptionDto>();
        [BindProperty]
        public int CityID { get; set; }
        public SelectList? Cities { get; set; }
        [BindProperty]
        public int[] FacilitiesIDs { get; set; }
        public SelectList? Facilities { get; set; }
        public string? StatusMessage { get; set; }

        public EditModel(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await _mediator.Send(new GetOfferQuery
            {
                ID = id
            });

            if (_currentUser.ID != result.AuthorID)
            {
                return RedirectToPage("/Forbidden");
            }

            Offer = new EditOfferViewModel
            {
                Title = result.Title,
                Description = result.Description,
                AddressLine = result.AddressLine,
                CityID = result.CityID,
                PostalCode = result.PostalCode,
                Photos = result.OfferPhotos
            };
            Offer.LodgingOptions = await _mediator.Send(new GetLodgingOptionsQuery
            {
                OfferID = id
            });

            var cities = await _mediator.Send(new GetCitiesQuery());
            Cities = new SelectList(cities, "ID", "Name");
            var facilities = await _mediator.Send(new GetFacilitiesQuery());
            Facilities = new SelectList(facilities, "ID", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, OfferDto Offer, IList<LodgingOptionDto> LodgingOptions)
        {
            if (Offer is not null)
            {
                await _mediator.Send(new UpdateOfferCommand
                {
                    ID = id,
                    Title = Offer.Title,
                    Description = Offer.Description,
                    LodgingType = Offer.LodgingType,
                    CityID = Offer.CityID,
                    AddressLine = Offer.AddressLine,
                    PostalCode = Offer.PostalCode,
                    LodgingOptionsDto = LodgingOptions
                });

                return RedirectToPage("/Offer/Details", new { id = id });
            }
            return Page();
        }
    }
}
