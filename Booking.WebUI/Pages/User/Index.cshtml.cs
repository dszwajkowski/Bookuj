using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Offers;
using Booking.Application.Features.Queries.Offers;
using Booking.Application.Features.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.WebUI.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<Booking.Domain.Entities.User> _userManager;

        public UserDto? UserDto { get; set; }
        public PaginatedList<OfferBriefDto>? Offers { get; set; }
        public OfferFilters OfferFilters { get; set; } = new OfferFilters();
        public string? StatusMessage { get; set; }

        public IndexModel(IMediator mediator, UserManager<Domain.Entities.User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            var filter = new OfferFilters() { AuthorID = user.Id };
            var query = new GetOffersWithFiltersQuery { Filters = filter };
            Offers = await _mediator.Send(query);

            UserDto = await _mediator.Send(new GetUserQuery { UserID = user.Id });

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteOfferAsync(int id, string username, OfferFilters filters)
        {
            try
            {
                await _mediator.Send(new ArchiveOfferCommand { ID = id, Archived = true });
            }
            catch (NotAuthorizedException)
            {
                StatusMessage = $"B��d: Nie masz uprawnie� �eby usun�� ofert� {id}.";
                return await OnGetAsync(username);
            }
            catch (Exception)
            {
                StatusMessage = $"B��d: Wyst�pi� b��d podczas usuwania oferty {id}. Je�li problem b�dzie dalej wyst�powa� skontaktuj si� z administracj�.";
                return await OnGetAsync(username);
            }

            StatusMessage = $"Pomy�lnie usuni�to ofert� numer {id}.";
            return await OnGetAsync(username);
        }
    }
}
