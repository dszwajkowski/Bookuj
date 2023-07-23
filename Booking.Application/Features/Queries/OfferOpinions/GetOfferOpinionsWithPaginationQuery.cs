using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Mappings;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using MediatR;

namespace Booking.Application.Features.Queries.OfferOpinions
{
    public class GetOfferOpinionsWithPaginationQuery : IRequest<PaginatedList<OfferOpinionDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int OfferID { get; set; }
    }

    public class GetOfferOpinionsWithPaginationQueryHandler : IRequestHandler<GetOfferOpinionsWithPaginationQuery, PaginatedList<OfferOpinionDto>>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetOfferOpinionsWithPaginationQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<OfferOpinionDto>> Handle(GetOfferOpinionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            if(request.PageNumber < 1) request.PageNumber = 1;

            return await _context.OfferOpinion
                .Where(op => op.OfferID == request.OfferID)
                .ProjectTo<OfferOpinionDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
