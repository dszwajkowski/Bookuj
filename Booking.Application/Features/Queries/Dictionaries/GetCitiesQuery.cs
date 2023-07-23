using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.Dictionaries
{
    public class GetCitiesQuery : IRequest<IEnumerable<City>>
    {

    }

    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, IEnumerable<City>>
    {
        private readonly IApplicationDataContext _context;

        public GetCitiesQueryHandler(IApplicationDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<City>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            return await _context.City.ToListAsync();
        }
    }
}
