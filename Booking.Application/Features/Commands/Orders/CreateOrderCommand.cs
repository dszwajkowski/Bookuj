using AutoMapper;
using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using Booking.Domain.Entities;
using MediatR;

namespace Booking.Application.Features.Commands.Orders
{
    public class CreateOrderCommand : IRequest<string>
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? AddressLine { get; set; }
        public string? PostalCode { get; set; }
        public int? CityID { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IEnumerable<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();
        public int PaymentMethodID { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public CreateOrderCommandHandler(IApplicationDataContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //var reservations = _mapper.Map<List<Reservation>>(request.Reservations);
            //var excluded = new[] { "property1", "property2" };
            var reservations = new List<Reservation>();

            // TODO: refactor
            foreach (var r in request.Reservations)
            {
                reservations.Add(await _context.Reservation.FindAsync(new object[] { r.ID }, cancellationToken));
            }

            var order = new Order
            {
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                AddressLine = request.AddressLine,
                PostalCode = request.PostalCode,
                CityID = request.CityID,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Reservations = reservations,
                UserID = _currentUser.ID,
                TotalPrice = reservations.Sum(r => r.TotalPrice),
                PaymentMethodID = request.PaymentMethodID,
                IsFinalized = new int[] { 1, 2 }.Contains(request.PaymentMethodID) ? true : false,
                DateCreated = DateTime.UtcNow,
                DateFinalized = new int[] { 1, 2 }.Contains(request.PaymentMethodID) ? DateTime.UtcNow : null
                //FirstName = request.FirstName ?? null,
                //SecondName = request.SecondName ?? null,
                //AddressLine = request.AddressLine ?? null,
                //PostalCode = request.PostalCode ?? null,
                //CityID = request.CityID ?? null,
                //Email = request.Email ?? null,
                //PhoneNumber = request.PhoneNumber ?? null,
                //Reservations = reservations,
                //UserID = _currentUser.ID ?? null,
                //TotalPrice = reservations.Sum(r => r.TotalPrice),
                //PaymentMethodID = request.PaymentMethodID,
                //IsFinalized = new int[] { 1, 2 }.Contains(request.PaymentMethodID) ? true : false,
                //DateCreated = DateTime.UtcNow,
                //DateFinalized = new int[] { 1, 2 }.Contains(request.PaymentMethodID) ? DateTime.UtcNow : null
            };
            //order.FirstName = request.FirstName;
            //order.SecondName = request.SecondName;
            //order.AddressLine = request.AddressLine;
            //order.PostalCode = request.PostalCode;
            //order.CityID = request.CityID;
            //order.Email = request.Email;
            //order.PhoneNumber = request.PhoneNumber;
            //order.Reservations = reservations;
            //order.UserID = _currentUser.ID;
            //order.TotalPrice = reservations.Sum(r => r.TotalPrice);
            //order.PaymentMethodID = request.PaymentMethodID;
            //order.IsFinalized = new int[] { 1, 2 }.Contains(request.PaymentMethodID) ? true : false;
            //order.DateCreated = DateTime.UtcNow;
            //order.DateFinalized = new int[] { 1, 2 }.Contains(request.PaymentMethodID) ? DateTime.UtcNow : null;

            foreach (var r in order.Reservations)
            {
                r.StatusID = new int[] { 1, 2 }.Contains(request.PaymentMethodID) ? 3 : 2;
                r.CartID = null;
            }

            _context.Order.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return order.ID;
        }
    }
}
