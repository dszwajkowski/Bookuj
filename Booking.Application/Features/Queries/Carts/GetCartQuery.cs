using Booking.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Dtos;

namespace Booking.Application.Features.Queries.Carts
{
    public class GetCartQuery : IRequest<CartDto>
    {
        public string? ID { get; set; }
    }

    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartDto>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetCartQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cart
                .Where(c => c.ID == request.ID)
                .ProjectTo<CartDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);
        }
    }
}
