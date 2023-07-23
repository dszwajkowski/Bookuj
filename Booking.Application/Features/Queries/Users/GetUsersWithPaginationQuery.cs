using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Mappings;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.Users
{
    public class GetUsersWithPaginationQuery : IRequest<PaginatedList<UserBriefDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }

    public class GetUsersWithPaginationQueryHandler : IRequestHandler<GetUsersWithPaginationQuery, PaginatedList<UserBriefDto>>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetUsersWithPaginationQueryHandler(IApplicationDataContext context, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<PaginatedList<UserBriefDto>> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users;
            var userDtos = new List<UserBriefDto>();
            var allRoles = await _roleManager.Roles.ToListAsync(cancellationToken);

            // TODO: do it after pagination
            foreach (var user in users)
            {
                var u = _mapper.Map<UserBriefDto>(user);
                foreach (var r in allRoles)
                {
                    if (await _userManager.IsInRoleAsync(user, r.Name))
                    {
                        u.Roles.Add(new UserRoleDto { Name = r.Name, IsInRole = true });
                    }
                    else
                    {
                        u.Roles.Add(new UserRoleDto { Name = r.Name, IsInRole = false });
                    }
                }
                userDtos.Add(u);
            }
            
            return userDtos
                .AsQueryable()
                .ToPaginatedList(request.PageNumber, request.PageSize);
        }
    }
}
