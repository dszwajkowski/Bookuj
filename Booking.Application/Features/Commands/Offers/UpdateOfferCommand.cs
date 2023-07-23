using AutoMapper;
using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;
using Booking.Domain.Entities;
using Booking.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Commands.Offers
{
    public class UpdateOfferCommand : IRequest
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public LodgingType LodgingType { get; set; }
        public int CityID { get; set; }
        public string? AddressLine { get; set; }
        public string? PostalCode { get; set; }
        public IList<LodgingOption>? LodgingOptions { get; set; } = new List<LodgingOption>();
        public IList<LodgingOptionDto>? LodgingOptionsDto { get; set; } = new List<LodgingOptionDto>();
    }

    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUser;

        public UpdateOfferCommandHandler(IApplicationDataContext context, IMapper mapper, UserManager<User> userManager, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _context.Offer
                .Where(o => o.ID == request.ID)
                .Include("City")
                .Include("LodgingOptions")
                .SingleAsync();

            if (offer == null) throw new NotFoundException();

            var currentUser = await _userManager.FindByIdAsync(_currentUser.ID);
            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (offer.AuthorId != _currentUser.ID && !isAdmin)
            {
                throw new NotAuthorizedException();
            }

            offer.Title = request.Title;
            offer.Description = request.Description;
            offer.LodgingType = request.LodgingType;
            offer.AddressLine = request.AddressLine;
            offer.PostalCode = request.PostalCode;
            offer.DateUpdated = DateTime.UtcNow;

            // TODO: Refactor
            //var lodgingOptionsToDelete = offer.LodgingOptions.Except(lodgingOptions).ToList();

            //foreach (var lo in lodgingOptionsToDelete)
            //{
            //    var lodgingOption = await _context.LodgingOption.FindAsync(new object[] { lo.ID });
            //    _context.LodgingOption.Remove(lodgingOption);
            //}

            var lodgingOptions = _mapper.Map<IList<LodgingOption>>(request.LodgingOptionsDto);

            offer.LodgingOptions = lodgingOptions;
            

            if (offer.City.ID != request.CityID)
            {
                var city = await _context.City
                    .FindAsync(new object[] { request.CityID }, cancellationToken);
                
                offer.City = city;
            }
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
        