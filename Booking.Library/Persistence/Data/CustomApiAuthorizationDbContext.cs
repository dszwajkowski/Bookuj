using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Booking.Infrastructure.Persistence.Data
{
    public class CustomApiAuthorizationDbContext<TUser, TRole, TUserRole, TKey> : IdentityDbContext<TUser, TRole, TKey, IdentityUserClaim<TKey>, TUserRole, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>, IPersistedGrantDbContext, IDisposable 
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        //
        // Summary:
        //     Gets or sets the Microsoft.EntityFrameworkCore.DbSet`1.
        public DbSet<PersistedGrant> PersistedGrants
        {
            get;
            set;
        }

        //
        // Summary:
        //     Gets or sets the Microsoft.EntityFrameworkCore.DbSet`1.
        public DbSet<DeviceFlowCodes> DeviceFlowCodes
        {
            get;
            set;
        }

        //
        // Summary:
        //     Gets or sets the Microsoft.EntityFrameworkCore.DbSet`1.
        public DbSet<Key> Keys
        {
            get;
            set;
        }

        //
        // Summary:
        //     Initializes a new instance of Microsoft.AspNetCore.ApiAuthorization.IdentityServer.ApiAuthorizationDbContext`1.
        //
        // Parameters:
        //   options:
        //     The Microsoft.EntityFrameworkCore.DbContextOptions.
        //
        //   operationalStoreOptions:
        //     The Microsoft.Extensions.Options.IOptions`1.
        public CustomApiAuthorizationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options)
        {
            _operationalStoreOptions = operationalStoreOptions;
        }

        Task<int> IPersistedGrantDbContext.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
        }
    }
}
