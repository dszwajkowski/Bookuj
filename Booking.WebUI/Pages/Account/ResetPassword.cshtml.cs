using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Booking.WebUI.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<Booking.Domain.Entities.User> _userManager;

        public ResetPasswordModel(UserManager<Booking.Domain.Entities.User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Adres e-mail nie mo�e by� pusty.")]
            [EmailAddress]
            [Display(Name = "Adres e-mail")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Has�o nie mo�e by� puste.")]
            [StringLength(100, ErrorMessage = "{0} musi zawiera� od {2} do {1} znak�w.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Has�o")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Has�o nie mo�e by� puste.")]
            [DataType(DataType.Password)]
            [Display(Name = "Potwierd� has�o")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required(ErrorMessage = "Kod do zresetowania has�a jest niepoprawny.")]
            [Display(Name = "Kod resetowania has�a")]
            public string Code { get; set; }

        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                StatusMessage = "Pomy�lnie zresetowano has�o";
                return Page();
                //return RedirectToPage("./ResetPasswordConfirmation");
                
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                StatusMessage = "Pomy�lnie zresetowano has�o";
                return Page();
                //return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
