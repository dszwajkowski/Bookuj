using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Common.Mappings;

namespace Booking.Application.Features.Queries.Offers
{
    public class GetOffersWithFiltersQuery : IRequest<PaginatedList<OfferBriefDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public OfferFilters Filters { get; set; } = new OfferFilters();
    }

    public class GetOffersWithFiltersQueryHandler : IRequestHandler<GetOffersWithFiltersQuery, PaginatedList<OfferBriefDto>>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetOffersWithFiltersQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<OfferBriefDto>> Handle(GetOffersWithFiltersQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber < 1) request.PageNumber = 1;
            if (request.PageSize < 1) request.PageSize = 5;
            if (request.PageSize > 100) request.PageSize = 100;

            var offersFromSp = _context.GetFilteredOffersFromStoredProcedure(request.Filters)
                .ToList()
                .AsQueryable();
                //.ProjectTo<OfferBriefDto>(_mapper.ConfigurationProvider);

            var offers = _mapper.Map<List<OfferBriefDto>>(offersFromSp);

            //InvalidOperationException: The provider for the source 'IQueryable' doesn't implement 'IAsyncQueryProvider'
            //return await offers.ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return await Task.FromResult<PaginatedList<OfferBriefDto>>(offers.ToPaginatedList(request.PageNumber, request.PageSize));
        }
    }
}
