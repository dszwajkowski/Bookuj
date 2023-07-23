using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.Users
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public string UserID { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Where(u => u.Id == request.UserID)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleAsync();
        }
    }
}
