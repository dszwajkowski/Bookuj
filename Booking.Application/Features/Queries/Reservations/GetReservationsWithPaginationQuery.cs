using AutoMapper.QueryableExtensions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Common.Mappings;
using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.Reservations
{
    public class GetReservationsWithPaginationQuery : IRequest<PaginatedList<ReservationDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public string? UserID { get; set; }
        public int? Status { get; set; }
    }

    public class GetReservationsWithPaginationQueryHandler : IRequestHandler<GetReservationsWithPaginationQuery, PaginatedList<ReservationDto>>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserManagerService _userManagerService;

        public GetReservationsWithPaginationQueryHandler(IApplicationDataContext context, IMapper mapper, 
            ICurrentUserService currentUserService, IUserManagerService userManagerService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userManagerService = userManagerService;
        }

        public async Task<PaginatedList<ReservationDto>> Handle(GetReservationsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber < 1) request.PageNumber = 1;
            if (request.PageSize < 1) request.PageSize = 10;

            var reservations = _context.Reservation.AsQueryable();
            var reservations2 = _context.Reservation
                .Include(r => r.LodgingOption.Offer)
                .AsQueryable();

            if (request.UserID is not null)
            {
                reservations = reservations.Where(r => r.LodgingOption.Offer.AuthorId == request.UserID);
            }
                
            if (request.Status is not null)
            {
                reservations = reservations.Where(r => r.Status.ID == request.Status);
            }

            return await reservations
                .ProjectTo<ReservationDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync<ReservationDto>(request.PageNumber, request.PageSize);
        }
    }
}
