using Booking.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Dtos;

namespace Booking.Application.Features.Queries.Carts
{
    public class GetSessionCartQuery : IRequest<CartDto>
    {
        public string? SessionID { get; set; }
    }

    public class GetSessionCartQueryHandler : IRequestHandler<GetSessionCartQuery, CartDto>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetSessionCartQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(GetSessionCartQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cart
                .Where(c => c.SessionID == request.SessionID)
                .ProjectTo<CartDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);
        }
    }
}
