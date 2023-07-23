using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Queries.Dictionaries
{
    public class GetPaymentMethodsQuery : IRequest<IEnumerable<PaymentMethod>>
    {

    }

    public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, IEnumerable<PaymentMethod>>
    {
        private readonly IApplicationDataContext _context;

        public GetPaymentMethodsQueryHandler(IApplicationDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentMethod>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            return await _context.PaymentMethod.ToListAsync();
        }
    }
}
