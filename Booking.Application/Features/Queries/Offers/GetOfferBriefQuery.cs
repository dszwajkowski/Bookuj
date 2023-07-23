using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Dtos;

namespace Booking.Application.Features.Queries.Offers
{
    public class GetOfferBriefQuery : IRequest<OfferBriefDto>
    {
        public int ID { get; set; }
    }

    public class GetOfferBriefQueryHandler : IRequestHandler<GetOfferBriefQuery, OfferBriefDto>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetOfferBriefQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OfferBriefDto> Handle(GetOfferBriefQuery request, CancellationToken cancellationToken)
        {
            var offer = await _context.Offer
                .Where(o => o.ID == request.ID)
                .ProjectTo<OfferBriefDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);  
            
            if(offer is null)
            {
                throw new NotFoundException(nameof(Offer), request.ID);
            }

            return offer;
        }
    }
}
