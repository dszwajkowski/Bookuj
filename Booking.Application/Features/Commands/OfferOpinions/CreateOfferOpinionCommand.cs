using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using MediatR;

namespace Booking.Application.Features.Commands.OfferOpinions
{
    public class CreateOfferOpinionCommand : IRequest<int>
    {
        public string UserID { get; set; }
        public int OfferID { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }

    public class CreateOfferOpinionCommandHandler : IRequestHandler<CreateOfferOpinionCommand, int>
    {
        private readonly IApplicationDataContext _context;

        public CreateOfferOpinionCommandHandler(IApplicationDataContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateOfferOpinionCommand request, CancellationToken cancellationToken)
        {
            var offerOpinion = new OfferOpinion
            {
                AuthorID = request.UserID,
                OfferID = request.OfferID,
                Text = request.Text,
                Rating = request.Rating,
                DateCreated = DateTime.UtcNow
            };

            _context.OfferOpinion.Add(offerOpinion);
            await _context.SaveChangesAsync(cancellationToken);

            return offerOpinion.ID;
        }
    }
}
