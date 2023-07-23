using Booking.Application.Features.Commands.Carts;
using Booking.Application.Features.Queries.Carts;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Booking.WebUI.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Booking.Domain.Entities.User> _signInManager;
        private readonly UserManager<Booking.Domain.Entities.User> _userManager;
        private readonly IMediator _mediator;

        public LoginModel(SignInManager<Booking.Domain.Entities.User> signInManager, UserManager<Domain.Entities.User> userManager, IMediator mediator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mediator = mediator;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Podaj adres e-mail.")]
            [EmailAddress]
            [Display(Name = "Adres e-mail")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Podaj has³o.")]
            [DataType(DataType.Password)]
            [Display(Name = "Has³o")]
            public string Password { get; set; }

            [Display(Name = "Czy zapamiêtaæ?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Nieudana próba logowania.");
                    return Page();
                }                
                if (user.IsBlocked)
                {
                    ModelState.AddModelError(string.Empty, $"Zosta³eœ zablokowany do {user.LockUntil}.");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    //_logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nieudana próba logowania.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
