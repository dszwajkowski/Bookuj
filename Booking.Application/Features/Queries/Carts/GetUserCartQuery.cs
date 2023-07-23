using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Dtos;

namespace Booking.Application.Features.Queries.Carts
{
    public class GetUserCartQuery : IRequest<CartDto>
    {
        public string? UserID { get; set; }
    }

    public class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, CartDto>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetUserCartQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cart
                .Where(c => c.UserID == request.UserID)
                .ProjectTo<CartDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);
        }
    }
}
