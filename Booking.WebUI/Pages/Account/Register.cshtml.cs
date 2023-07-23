using Booking.Application.Features.Commands.Carts;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

#nullable disable

namespace Booking.WebUI.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Booking.Domain.Entities.User> _signInManager;
        private readonly UserManager<Booking.Domain.Entities.User> _userManager;
        private readonly IUserStore<Booking.Domain.Entities.User> _userStore;
        private readonly IUserEmailStore<Booking.Domain.Entities.User> _emailStore;
        private readonly IUserPhoneNumberStore<Booking.Domain.Entities.User> _phoneNumberStore;
        private readonly IEmailSender _emailSender;
        private readonly IMediator _mediator;

        public RegisterModel(
            UserManager<Booking.Domain.Entities.User> userManager,
            IUserStore<Booking.Domain.Entities.User> userStore,
            SignInManager<Booking.Domain.Entities.User> signInManager,
            IEmailSender emailSender, 
            IMediator mediator)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _emailSender = emailSender;
            _phoneNumberStore = GetPhoneStore();
            _mediator = mediator;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public SelectList? Cities { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Nazwa u¿ytkownika nie mo¿e byæ pusta.")]
            [Display(Name = "Nazwa u¿ytkownika*")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Adres e-mail nie mo¿e byæ pusty.")]
            [EmailAddress(ErrorMessage = "Podany adres e-mail jest niepoprawny.")]
            [Display(Name = "Adres e-mail*")]
            public string Email { get; set; }
            
            [Required(ErrorMessage = "Numer telefonu nie mo¿e byæ pusty.")]
            [Phone(ErrorMessage = "Podany numer telefonu jest niepoprawny.")]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; set; }

            //[Display(Name = "Imiê")]
            //public string FirstName { get; set; }

            //[Display(Name = "Nazwisko")]
            //public string SecondName { get; set; }

            //[Display(Name = "Adres")]
            //public string AddressLine { get; set; }

            //[Display(Name = "Miasto")]
            //public int? CityID { get; set; }

            //[Display(Name = "Kod pocztowy")]
            //public string PostalCode { get; set; }

            [Required(ErrorMessage = "Has³o nie mo¿e byæ puste.")]
            [StringLength(100, ErrorMessage = "{0} musi zawieraæ od {2} do {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Has³o*")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Has³o nie mo¿e byæ puste.")]
            [DataType(DataType.Password)]
            [Display(Name = "PotwierdŸ has³o*")]
            [Compare(nameof(Password), ErrorMessage = "Podane has³a s¹ ró¿ne.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();
                //user.FirstName = String.IsNullOrWhiteSpace(Input.FirstName) ? null : Input.FirstName;
                //user.SecondName = String.IsNullOrWhiteSpace(Input.SecondName) ? null : Input.SecondName;
                //user.AddressLine = String.IsNullOrWhiteSpace(Input.AddressLine) ? null : Input.AddressLine;
                //user.PostalCode = String.IsNullOrWhiteSpace(Input.PostalCode) ? null : Input.PostalCode;
                //user.CityID = Input.CityID is null || Input.CityID == 0 ? null : Input.CityID;
                await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                await _phoneNumberStore.SetPhoneNumberAsync(user, Input.PhoneNumber, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    //_logger.LogInformation("User created a new account with password.");
                    user.AvatarID = 1;
                    var userId = await _userManager.GetUserIdAsync(user);
                    
                    await _mediator.Send(new CreateCartCommand { UserID = userId });

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "PotwierdŸ adres e-mail",
                        $"PotwierdŸ swoje konto <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikaj¹c tutaj</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        // TODO: Confirm registration message
                        StatusMessage = "Pomyœlnie zarejestrowano, link aktywacyjny zosta³ przes³any na podany adres e-mail.";
                        return Page();
                        //return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private Booking.Domain.Entities.User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Booking.Domain.Entities.User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Booking.Domain.Entities.User)}'. " +
                    $"Ensure that '{nameof(Booking.Domain.Entities.User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Booking.Domain.Entities.User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("User store with email support is required.");
            }
            return (IUserEmailStore<Booking.Domain.Entities.User>)_userStore;
        }

        private IUserPhoneNumberStore<Booking.Domain.Entities.User> GetPhoneStore()
        {
            if (!_userManager.SupportsUserPhoneNumber)
            {
                throw new NotSupportedException("User store with phone number support is required.");
            }
            return (IUserPhoneNumberStore<Booking.Domain.Entities.User>)_userStore;
        }
    }
}
