using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Security;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Booking.Application.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ICurrentUserService _currentUser;
        // TODO: create identity service
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public AuthorizationBehaviour(ICurrentUserService currentUser, UserManager<User> userManager)//, ILogger logger)
        {
            _currentUser = currentUser;
            _userManager = userManager;
            //_logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {
                if (_currentUser.ID is null)
                {
                    _logger.LogError("Unauthorized access to resource by not logged in user");  
                    throw new NotAuthorizedException();
                }

                var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

                if (authorizeAttributesWithRoles.Any())
                {
                    var authorized = false;

                    foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                    {
                        var user = await _userManager.FindByIdAsync(_currentUser.ID);

                        foreach (var role in roles)
                        {
                            var isInRole = await _userManager.IsInRoleAsync(user, role.Trim());
                            if (isInRole)
                            {
                                authorized = true;
                                break;
                            }
                        }
                    }

                    if (!authorized)
                    {
                        _logger.LogError("Unauthorized access by {User}", new object[] { _currentUser });
                        throw new NotAuthorizedException();
                    }
                }
            }

            return await next();
        }
    }
}
