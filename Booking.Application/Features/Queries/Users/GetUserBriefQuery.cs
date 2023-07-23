using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.Users
{
    public class GetUserBriefQuery : IRequest<UserBriefDto>
    {
        public string UserID { get; set; }
    }

    public class GetUserBriefQueryHandler : IRequestHandler<GetUserBriefQuery, UserBriefDto>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetUserBriefQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserBriefDto> Handle(GetUserBriefQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Where(u => u.Id == request.UserID)
                .ProjectTo<UserBriefDto>(_mapper.ConfigurationProvider)
                .SingleAsync();
        }
    }
}
