using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.WebUI.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Booking.Domain.Entities.User> _signInManager;
        //private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<Booking.Domain.Entities.User> signInManager)//, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
           // _logger = logger;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            //_logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
