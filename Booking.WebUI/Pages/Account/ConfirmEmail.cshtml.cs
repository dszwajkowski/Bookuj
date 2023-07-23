using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

#nullable disable

namespace Booking.WebUI.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<Booking.Domain.Entities.User> _userManager;

        public ConfirmEmailModel(UserManager<Booking.Domain.Entities.User> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Nie znaleziono u¿ytkownika '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Dziêkujemy za potwierdzenie twojego adresu e-mail." : "Wyst¹pi³ b³¹d podczas potwierdzania twojego adresu e-mail.";
            return Page();
        }
    }
}
