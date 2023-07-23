using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Booking.Infrastructure.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<User> _userManager;

        public UserManagerService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User> FindByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> IsInRoleAsync(string userID, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userID);

            if (user is null)
            {
                throw new NotFoundException();
            }

            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles)
        {
            return await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles)
        {
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

    }
}
