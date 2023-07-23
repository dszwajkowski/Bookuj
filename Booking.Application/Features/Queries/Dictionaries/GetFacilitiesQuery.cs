using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.Dictionaries
{
    public class GetFacilitiesQuery : IRequest<IEnumerable<City>>
    {

    }

    public class GetFacilitiesQueryHandler : IRequestHandler<GetFacilitiesQuery, IEnumerable<City>>
    {
        private readonly IApplicationDataContext _context;

        public GetFacilitiesQueryHandler(IApplicationDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<City>> Handle(GetFacilitiesQuery request, CancellationToken cancellationToken)
        {
            return await _context.City.ToListAsync();
        }
    }
}
