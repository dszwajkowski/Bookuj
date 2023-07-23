using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Users;
using Booking.Application.Features.Queries.Dictionaries;
using Booking.Application.Features.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

#nullable disable

namespace Booking.WebUI.Pages.Profile.Edit
{
    public class ProfileModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        [BindProperty]
        public UserDto UserDto { get; set; }
        public SelectList Cities { get; set; }
        public string StatusMessage { get; set; }

        public ProfileModel(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            UserDto = await _mediator.Send(new GetUserQuery
            {
                UserID = _currentUser.ID
            });

            if (UserDto == null)
            {
                return NotFound($"Nie znaleziono u¿ytkownika o ID '{_currentUser.ID}'.");
            }

            var cities = await _mediator.Send(new GetCitiesQuery());
            Cities = new SelectList(cities, "ID", "Name");

            return Page();
        }

        public async Task OnPostAsync()
        {
            await _mediator.Send(new UpdateUserCommand
            {
                UserID = _currentUser.ID,
                Email = UserDto.Email,
                PhoneNumber = UserDto.PhoneNumber,
                FirstName = UserDto.FirstName,
                SecondName = UserDto.SecondName,
                AddressLine = UserDto.AddressLine,
                PostalCode = UserDto.PostalCode,
                CityID = UserDto.CityID,
                Description = UserDto.Description,
                Avatar = UserDto.Avatar
            });

            await OnGetAsync();
        }
    }
}
