using Booking.Application.Common.Models;
using Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Common.Interfaces
{
    public interface IApplicationDataContext
    {
        DbSet<User> Users { get; set; }
        //DbSet<IdentityRole> Roles { get; set; }
        //DbSet<IdentityUserRole<string>> UserRole { get; set; }
        DbSet<Offer> Offer { get; set; }
        DbSet<LodgingOption> LodgingOption { get; set; }
        DbSet<LodgingFacilities> LodgingFacilities { get; set; }
        DbSet<OfferOpinion> OfferOpinion { get; set; }
        DbSet<City> City { get; set; }
        DbSet<Region> Region { get; set; }
        DbSet<Country> Country { get; set; }
        DbSet<Reservation> Reservation { get; set; }
        DbSet<ReservationStatus> ReservationStatus { get; set; }
        DbSet<Cart> Cart { get; set; }
        DbSet<OfferFromStoredProcedure> OfferFromStoredProcedure { get; set; }
        DbSet<OfferPhoto> OfferPhotos { get; set; }
        DbSet<UserAvatar> UserAvatar { get; set; }
        DbSet<PaymentMethod> PaymentMethod { get; set; }
        DbSet<Order> Order { get; set; }
        IEnumerable<OfferFromStoredProcedure> GetFilteredOffersFromStoredProcedure(OfferFilters filters, bool filterBlockedUsers = true);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);    
    }
}
