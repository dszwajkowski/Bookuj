using AutoMapper;
using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Carts;
using Booking.Application.Features.Commands.Orders;
using Booking.Application.Features.Queries.Carts;
using Booking.Application.Features.Queries.Dictionaries;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Booking.WebUI.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<Booking.Domain.Entities.User> _userManager;
        private readonly ICurrentUserService _currentUser;

        public CartDto? Cart { get; set; }
        public OrderDto? Order { get; set; }
        public SelectList? Cities { get; set; }
        public SelectList? PaymentMethods { get; set; }
        [BindProperty]
        public int PaymentMethodID { get; set; }
        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            [Required(ErrorMessage = "Imiê nie mo¿e byæ puste.")]
            [Display(Name = "Imiê")]
            public string? FirstName { get; set; }
            [Required(ErrorMessage = "Nazwisko nie mo¿e byæ puste.")]
            [Display(Name = "Nazwisko")]
            public string? SecondName { get; set; }
            [Required(ErrorMessage = "Adres nie mo¿e byæ pusty.")]
            [Display(Name = "Adres")]
            public string? AddressLine { get; set; }
            [Required(ErrorMessage = "Kod pocztowy nie mo¿e byæ pusty.")]
            [Display(Name = "Kod pocztowy")]
            public string? PostalCode { get; set; }
            [Required(ErrorMessage = "Miasto nie mo¿e byæ puste.")]
            [Display(Name = "Miasto")]
            public int? CityID { get; set; }
            [Required(ErrorMessage = "Adres e-mail nie mo¿e byæ pusty.")]
            [EmailAddress(ErrorMessage = "Podany adres e-mail jest niepoprawny.")]
            [Display(Name = "Adres e-mail")]
            public string? Email { get; set; }
            [Required(ErrorMessage = "Numer telefonu nie mo¿e byæ pusty.")]
            [Phone(ErrorMessage = "Podany numer telefonu jest niepoprawny.")]
            [Display(Name = "Numer telefonu")]
            public string? PhoneNumber { get; set; }
        }

        public CheckoutModel(IMediator mediator, UserManager<Domain.Entities.User> userManager, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                bool isAuthenticated = User.Identity is not null && User.Identity.IsAuthenticated;

                Cart = isAuthenticated
                    ? await _mediator.Send(new GetUserCartQuery { UserID = _currentUser.ID })
                    : await _mediator.Send(new GetSessionCartQuery { SessionID = HttpContext.Session.Id });
            }
            catch (InvalidOperationException)
            {
                return RedirectToPage("/Forbidden");
            }

            var cities = await _mediator.Send(new GetCitiesQuery());
            Cities = new SelectList(cities, "ID", "Name");
            var paymentMethods = await _mediator.Send(new GetPaymentMethodsQuery());
            PaymentMethods = new SelectList(paymentMethods, "ID", "Name");

            Booking.Domain.Entities.User user = null;
            try
            {
                user = await _userManager.FindByIdAsync(_currentUser.ID);
            }
            catch (Exception) { }

            if (user is not null)
            {
                Input.FirstName = user.FirstName;
                Input.SecondName = user.SecondName;
                Input.AddressLine = user.AddressLine;
                Input.CityID = user.CityID;
                Input.PostalCode = user.PostalCode;
                Input.Email = user.Email;
                Input.PhoneNumber = user.PhoneNumber;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CartDto cart, int paymentMethodID, InputModel input)
        {
            var result = await _mediator.Send(new CreateOrderCommand
            {
                FirstName = input.FirstName,
                SecondName = input.SecondName,
                AddressLine = input.AddressLine,
                PostalCode = input.PostalCode,
                CityID = input.CityID,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
                Reservations = cart.Reservations,
                PaymentMethodID = paymentMethodID,
            });

            if (paymentMethodID == 3)
            {
                return RedirectToPage("/Cart/CardPayment", new { orderID = result });
            }

            return RedirectToPage("/Order/Details", new { orderID = result });
        }
    }
}
