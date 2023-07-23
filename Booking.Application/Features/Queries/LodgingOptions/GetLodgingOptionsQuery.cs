using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.LodgingOptions
{
    public class GetLodgingOptionsQuery : IRequest<IList<LodgingOptionDto>>
    {
        public int OfferID { get; set; }
        public OfferFilters Filters { get; set; } = new OfferFilters();
    }

    public class GetLodgingOptionsQueryHandler : IRequestHandler<GetLodgingOptionsQuery, IList<LodgingOptionDto>>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetLodgingOptionsQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<LodgingOptionDto>> Handle(GetLodgingOptionsQuery request, CancellationToken cancellationToken)
        {
            var lodgingOptions = await 
                        (from lo in _context.LodgingOption
                         where lo.Offer.ID == request.OfferID
                            && (lo.PersonCount == request.Filters.PersonCount || request.Filters.PersonCount == null)
                            && (lo.RoomCount == request.Filters.RoomCount || request.Filters.RoomCount == null)
                            && (lo.BedCount == request.Filters.BedCount || request.Filters.BedCount == null)
                            && (lo.Price >= request.Filters.PriceMin || request.Filters.PriceMin == null)
                            && (lo.Price <= request.Filters.PriceMax || request.Filters.PriceMax == null)
                            && (lo.Size >= request.Filters.SizeMin || request.Filters.SizeMin == null)
                            && (lo.Size <= request.Filters.SizeMax || request.Filters.SizeMax == null)
                            && !_context.Reservation.Any(r => r.LodgingOption.ID == lo.ID
                                                            && r.DateFrom < request.Filters.DateTo
                                                            && r.DateTo > request.Filters.DateFrom
                                                            && (r.StatusID == 2 || r.StatusID == 3))
                            select lo)
                        .ProjectTo<LodgingOptionDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();

            return lodgingOptions;
        }
    }
}
