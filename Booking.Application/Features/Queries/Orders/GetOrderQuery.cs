using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.Orders
{
    public class GetOrderQuery : IRequest<OrderDto>
    {
        public string ID { get; set; }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetOrderQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            return await _context.Order
                .Where(o => o.ID == request.ID)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .SingleAsync();
        }
    }
}
