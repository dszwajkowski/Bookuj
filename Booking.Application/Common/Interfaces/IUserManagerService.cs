using Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Booking.Application.Common.Interfaces
{
    public interface IUserManagerService
    {
        Task<User> FindByIdAsync(string id);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByUsername(string username);
        Task<bool> IsInRoleAsync(string userID, string roleName);
        Task<bool> IsInRoleAsync(User user, string roleName);
        Task<IList<string>> GetRolesAsync(User user);
        Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles);
        Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
    }
}
