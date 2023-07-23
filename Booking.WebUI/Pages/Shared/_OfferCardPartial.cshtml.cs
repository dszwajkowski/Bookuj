using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Offers;
using MediatR;

namespace Booking.WebUI.Pages.Shared
{
    public class _OfferCardPartialModel
    {
        public OfferBriefDto Offer { get; set; }
        public OfferFilters Filters { get; set; }
        public string DeleteHandler { get; set; }

        public _OfferCardPartialModel(OfferBriefDto offer, OfferFilters filters, string deleteHandler = "DeleteOffer")
        {
            Offer = offer;
            Filters = filters;
            DeleteHandler = deleteHandler;
        }
    }
}
