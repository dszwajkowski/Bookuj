using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Carts;
using Booking.Application.Features.Commands.OfferOpinions;
using Booking.Application.Features.Commands.Reservations;
using Booking.Application.Features.Queries.Carts;
using Booking.Application.Features.Queries.LodgingOptions;
using Booking.Application.Features.Queries.OfferOpinions;
using Booking.Application.Features.Queries.Offers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.WebUI.Pages.Offer
{
    public class DetailsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public OfferDto Offer { get; set; } = new OfferDto();
        public ICollection<LodgingOptionDto>? LodgingOptions { get; set; }
        [BindProperty]
        public OfferFilters Filters { get; set; } = new OfferFilters();
        [BindProperty]
        public int OfferOpinionRating { get; set; }
        [BindProperty]
        public string OfferOpinionText { get; set; }
        public PaginatedList<OfferOpinionDto> OfferOpinions { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public DetailsModel(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGetAsync(int id, string? dateFrom, string? dateTo, [FromQuery] OfferFilters filters, int pageNumber = 1)
        {
            Filters = filters;

            await GetOffer(id);

            if (dateFrom is not null && dateTo is not null)
            {
                Filters.DateFrom = Convert.ToDateTime(dateFrom);
                Filters.DateTo = Convert.ToDateTime(dateTo);
                await GetOfferLodgingOptions(id, Filters);
            }
            else if (Filters.DateFrom is not null && Filters.DateTo is not null)
            {
                await GetOfferLodgingOptions(id, Filters);
            }
            await GetOfferOpinions(id, pageNumber);

            return Page();
        }

        public async Task OnPostAsync(int id, int pageNumber, OfferFilters filters)
        {
            await OnGetAsync(id, null, null, filters, pageNumber);
        }

        public async Task OnPostLoadMoreOpinionsAsync(int id, int pageNumber, OfferFilters filters)
        {
            //DateFrom = Filters.DateFrom;
            //DateTo = Filters.DateTo;

            await OnGetAsync(id, null, null, filters, pageNumber);
        }

        public async Task OnPostAddToCartAsync(int lodgingOptionsID, string? dateFrom, string? dateTo)
        {
            string cartID;
            bool isAuthenticated = User.Identity is not null && User.Identity.IsAuthenticated;

            try
            {
                var cart = isAuthenticated
                    ? await _mediator.Send(new GetUserCartQuery { UserID = _currentUser.ID })
                    : await _mediator.Send(new GetSessionCartQuery { SessionID = HttpContext.Session.Id });
                cartID = cart.ID;
            }
            catch (Exception)
            {
                cartID = await _mediator.Send(new CreateCartCommand { SessionID = HttpContext.Session.Id });
            }
            // save anything to session to make sure session id stays the same between requests
            HttpContext.Session.SetString("CartID", cartID);

            await _mediator.Send(new CreateReservationCommand
            {
                DateFrom = Convert.ToDateTime(dateFrom),
                DateTo = Convert.ToDateTime(dateTo),
                LodgingOptionID = lodgingOptionsID,
                CartID = cartID
            });
        }

        public async Task<IActionResult> OnPostDeleteOpinionAsync(int id, string dateFrom, string dateTo, OfferFilters filters, int pageNumber, int opinionID)
        {
            try
            {
                await _mediator.Send(new DeleteOfferOpinionCommand { ID = opinionID });
            }
            catch (NotAuthorizedException)
            {
                StatusMessage = $"B³¹d: Nie masz uprawnieñ ¿eby usun¹æ opiniê {opinionID}";
                return await OnGetAsync(id, dateFrom, dateTo, filters, pageNumber);
            }
            catch (Exception)
            {
                StatusMessage = StatusMessage = $"B³¹d: Wyst¹pi³ b³¹d podczas usuwania opinii {opinionID}. Jeœli problem bêdzie dalej wystêpowaæ skontaktuj siê z administracj¹.";
                return await OnGetAsync(id, dateFrom, dateTo, filters, pageNumber);
            }

            StatusMessage = $"Pomyœlnie usuniêto opiniê {opinionID}.";

            return await OnGetAsync(id, dateFrom, dateTo, filters, pageNumber);
        }

        private async Task GetOffer(int id)
        {
            Offer = await _mediator.Send(new GetOfferQuery { ID = id });
        }

        private async Task GetOfferLodgingOptions(int id, OfferFilters filters)
        {
            LodgingOptions = await _mediator.Send(new GetLodgingOptionsQuery
            {
                OfferID = id,
                Filters = filters
            });
        }

        private async Task GetOfferOpinions(int id, int pageNumber = 1, int pageSize = 5)
        {
            OfferOpinions = await _mediator.Send(new GetOfferOpinionsWithPaginationQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                OfferID = id
            });
        }
    }
}
