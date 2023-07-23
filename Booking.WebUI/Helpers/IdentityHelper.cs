using System.Security.Claims;

namespace Booking.WebUI.Helpers
{
    public static class IdentityHelper
    {
        public static string GetLoggedUserId(this HttpContext context)
        {
            return context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
