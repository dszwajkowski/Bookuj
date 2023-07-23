using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Orders;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Booking.WebUI.Pages.Cart
{
    public class CardPaymentModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<Booking.Domain.Entities.User> _userManager;
        private readonly ICurrentUserService _currentUser;

        public OrderDto? Order { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();
        public string? OrderID { get; set; }

        public class InputModel
        {
            [CreditCard(ErrorMessage = "Podany numer karty nie jest poprawny.")]
            [Required(ErrorMessage = "Numer karty nie mo¿e byæ pusty.")]
            [Display(Name = "Numer karty kredytowej/debetowej")]
            public string? CardNumber { get; set; }
            [Required(ErrorMessage = "Data wygaœniecia karty nie mo¿e byæ pusta.")]
            [DataType(DataType.Date)]
            [Display(Name = "Data wygaœniecia karty")]
            public DateTime? ExpirationDate { get; set; }
            [Required(ErrorMessage = "CVV nie mo¿e byæ pusty.")]
            [Display(Name = "CVV")]
            public int? Cvv { get; set; }
        }

        public CardPaymentModel(IMediator mediator, UserManager<Domain.Entities.User> userManager, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public IActionResult OnGetAsync(string orderID)
        {
            OrderID = orderID;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? orderID)
        {
            if (!String.IsNullOrWhiteSpace(orderID))
            {
                await _mediator.Send(new UpdateOrderCommand { OrderID = orderID, IsFinalized = true });
            }

            return RedirectToPage("/Order/Details", new { orderID = orderID });
        }
    }
}
