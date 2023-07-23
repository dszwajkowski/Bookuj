using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Booking.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        //private readonly UserManager<User> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContext)//, UserManager<User> userManager)
        {
            _httpContext = httpContext;
            //_userManager = userManager;
        }

        public string ID => _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        //public async Task<bool> IsInRole(string roleName)
        //{
        //    var user = await _userManager.FindByIdAsync(ID);
        //    return await _userManager.IsInRoleAsync(user, roleName);
        //}
    }
}
