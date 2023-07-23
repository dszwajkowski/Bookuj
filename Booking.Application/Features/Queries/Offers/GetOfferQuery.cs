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
    public class GetOfferQuery : IRequest<OfferDto>
    {
        public int ID { get; set; }
    }

    public class GetOfferQueryHandler : IRequestHandler<GetOfferQuery, OfferDto>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetOfferQueryHandler(IApplicationDataContext context, 
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OfferDto> Handle(GetOfferQuery request, 
            CancellationToken cancellationToken)
        {
            var offer = await _context.Offer
                .Where(o => o.ID == request.ID)
                .ProjectTo<OfferDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);  
            
            if(offer is null)
            {
                throw new NotFoundException(nameof(Offer), request.ID);
            }

            return offer;
        }
    }
}
