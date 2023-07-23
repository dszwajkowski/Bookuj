using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using MediatR;
using AutoMapper;
using Booking.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace Booking.Application.Features.Queries.Reservations
{
    public class GetReservationQuery : IRequest<ReservationDto>
    {
        public int ID { get; set; }
    }

    public class GetReservationQueryHandler : IRequestHandler<GetReservationQuery, ReservationDto>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetReservationQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReservationDto> Handle(GetReservationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Reservation
                .Where(r => r.ID == request.ID)
                .ProjectTo<ReservationDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);
        }
    }
}
