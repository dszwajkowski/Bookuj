using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Mappings;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.Orders
{
    public class GetOrdersWithPaginationQuery : IRequest<PaginatedList<OrderDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string UserID { get; set; }
        //public string? AuthorID { get; set; }
    }

    public class GetOrdersWithPaginationQueryHandler : IRequestHandler<GetOrdersWithPaginationQuery, PaginatedList<OrderDto>>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetOrdersWithPaginationQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<OrderDto>> Handle(GetOrdersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Order
                .Where(o => o.UserID == request.UserID && (o.PaymentMethodID != null || o.PaymentMethodID != 0))
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
 