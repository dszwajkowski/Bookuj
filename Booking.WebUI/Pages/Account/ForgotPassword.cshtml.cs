using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

#nullable disable

namespace Booking.WebUI.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Booking.Domain.Entities.User> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<Booking.Domain.Entities.User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }


        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Adres e-mail nie mo¿e byæ pusty.")]
            [EmailAddress(ErrorMessage = "Podany adres e - mail jest niepoprawny")]
            [Display(Name = "Adres e-mail")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Resetowanie has³a",
                    $"Zresetuj swoje has³o <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikaj¹c tutaj</a>.");

                StatusMessage = "Jeœli podany adres e-mail istnieje u nas w systemie to zosta³a przes³ana na niego instrukcja jak zresetowaæ has³o.";
                return Page();

                //return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
