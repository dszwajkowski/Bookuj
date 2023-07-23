using AutoMapper;
using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Domain.Entities;
using Booking.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Booking.Application.Features.Commands.Offers
{
    public class CreateOfferCommand : IRequest<int>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AuthorID { get; set; }
        public LodgingType LodgingType { get; set; }
        public int? CityID { get; set; }
        public string? AddressLine { get; set; }
        public string? PostalCode { get; set; }
        public ICollection<LodgingOption>? LodgingOptions { get; set; } = new List<LodgingOption>();
        public ICollection<IFormFile>? Photos { get; set; } = new List<IFormFile>();
    }

    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, int>
    {
        private readonly IApplicationDataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;

        public CreateOfferCommandHandler(IApplicationDataContext context, UserManager<User> userManager, IFileManager fileManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            // TODO use FluentValidation
            if (request.LodgingOptions is null || request.LodgingOptions.Count == 0)
            {
                throw new ArgumentException("Oferta musi posiadać przynajmniej jedną opcję noclegową.");
            }
            if (request.Photos is null || request.Photos.Count == 0)
            {
                throw new ArgumentException("Oferta musi posiadać przynajmniej jedno zdjęcie.");
            }
            ArgumentNullException.ThrowIfNull(request.CityID);

            var author = await _userManager.FindByIdAsync(request.AuthorID);
            var city = await _context.City.FindAsync(new object[] { request.CityID });
            if (author is null || city is null)
            {
                throw new NotFoundException();
            }

            var offer = new Offer
            {
                Title = request.Title,
                Description = request.Description,
                Author = author,
                LodgingType = request.LodgingType,
                City = city,
                AddressLine = request.AddressLine,
                PostalCode = request.PostalCode,
                DateCreated = DateTime.UtcNow
            };

            _context.Offer.Add(offer);
            await _context.SaveChangesAsync(cancellationToken);

            // TODO: Refacotr
            foreach (var o in request.LodgingOptions)
            {
                    
                var lodgingsFacilities = new List<LodgingFacilities>();
                //foreach (var lo in o.LodgingFacilities)
                //{
                //    lodgingsFacilities.Add(_context.LodgingFacilities.Find(lo.ID));
                //}
                var lodgingOption = new LodgingOption()
                {
                    Price = o.Price,
                    PersonCount = o.PersonCount,
                    RoomCount = o.RoomCount,
                    BedCount = o.BedCount,
                    Size = o.Size,
                    Offer = offer,
                    LodgingFacilities = lodgingsFacilities
                };
                _context.LodgingOption.Add(lodgingOption);
                await _context.SaveChangesAsync(cancellationToken);
            }

            IEnumerable<FileModel> files;
            if (request.Photos.Count > 0)
            {
                files = await _fileManager.Upload(request.Photos, $@"images\offer\{offer.ID}");
                var offerPhotos = new List<OfferPhoto>();
                foreach (var file in files)
                {
                    offerPhotos.Add(_mapper.Map<OfferPhoto>(file));
                }
                //var offerPhots = _mapper.Map<OfferPhoto>(files);
                offer.OfferPhotos = offerPhotos;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return offer.ID;
        }
    }
}
