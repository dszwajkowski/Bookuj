using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Domain.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Booking.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDataContext //ApiAuthorizationDbContext<User>
    {
        public DbSet<Offer> Offer { get; set; }
        public DbSet<LodgingOption> LodgingOption { get; set; }
        public DbSet<LodgingFacilities> LodgingFacilities { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<ReservationStatus> ReservationStatus { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<OfferFromStoredProcedure> OfferFromStoredProcedure { get; set; }
        public DbSet<OfferPhoto> OfferPhotos { get; set; }
        public DbSet<UserAvatar> UserAvatar { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OfferOpinion> OfferOpinion { get; set; }

        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Reservation>()
                .HasOne(r => r.Cart)
                .WithMany(c => c.Reservations)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reservation>()
                .HasOne(r => r.Status)
                .WithMany(s => s.Reservations)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Order>()
                .HasOne(o => o.PaymentMethod)
                .WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OfferOpinion>()
               .HasOne(op => op.User)
               .WithMany(u => u.OfferOpinions)
               .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
               .HasOne(o => o.User)
               .WithMany(u => u.Orders)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .IsRequired(false);

            builder.Entity<Order>()
               .HasOne(o => o.City)
               .WithMany(c => c.Orders)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Offer>()
               .HasOne(o => o.City)
               .WithMany(c => c.Offers)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LodgingOption>().Property(o => o.Price).HasColumnType("decimal(18,4)");
            builder.Entity<Reservation>().Property(o => o.TotalPrice).HasColumnType("decimal(18,4)");
            builder.Entity<Order>().Property(o => o.TotalPrice).HasColumnType("decimal(18,4)");       
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public IEnumerable<OfferFromStoredProcedure> GetFilteredOffersFromStoredProcedure(OfferFilters filters, bool filterBlockedUsers = true)
        {
            var dateFrom = new SqlParameter("@DateFrom", filters.DateFrom ?? (object)DBNull.Value);
            var dateTo = new SqlParameter("@DateTo", filters.DateTo ?? (object)DBNull.Value);
            var city = new SqlParameter("@CityID", filters.City ?? (object)DBNull.Value);
            var lodgingType = new SqlParameter("@LodgingType", (object)DBNull.Value);
            var person = new SqlParameter("@PersonCount", filters.PersonCount ?? (object)DBNull.Value);
            var bed = new SqlParameter("@BedCount", filters.BedCount ?? (object)DBNull.Value);
            var room = new SqlParameter("@RoomCount", filters.RoomCount ?? (object)DBNull.Value);
            var priceMin = new SqlParameter("@PriceMin", filters.PriceMin ?? (object)DBNull.Value);
            var priceMax = new SqlParameter("@PriceMax", filters.PriceMax ?? (object)DBNull.Value);
            var sizeMin = new SqlParameter("@SizeMin", filters.SizeMin ?? (object)DBNull.Value);
            var sizeMax = new SqlParameter("@SizeMax", filters.SizeMax ?? (object)DBNull.Value);
            var author = new SqlParameter("@AuthorID", filters.AuthorID ?? (object)DBNull.Value);
            var filterBlocked = new SqlParameter("@FilterBlockedUsers", filterBlockedUsers == true ? 1 : 0);

            return OfferFromStoredProcedure
                .FromSqlRaw("exec sp_GetOffersWithFilters @DateFrom, @DateTo, @CityID, @LodgingType, @PersonCount, @BedCount, " +
                "@RoomCount, @PriceMin, @PriceMax, @SizeMin, @SizeMax, @AuthorID, @FilterBlockedUsers",
                    dateFrom, dateTo, city, lodgingType, person, bed, room, priceMin, priceMax, sizeMin, sizeMax, author, filterBlocked);
        }
    }
}
