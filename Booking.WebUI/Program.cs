using Booking.Application;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using Booking.Infrastructure.Persistence.Data;
using Booking.Infrastructure.Services;
using Bookuj.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var assembly = typeof(ApplicationDbContext).Assembly.FullName;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
        x => x.MigrationsAssembly(assembly)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, IdentityRole>>();

builder.Services.AddScoped<IApplicationDataContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddSingleton<IFileManager, FileManager>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped(typeof(IUserManagerService), typeof(UserManagerService));
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Forbidden";
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
});
var smtpSettings = new SmtpSettings();
builder.Configuration.Bind(nameof(smtpSettings), smtpSettings);
builder.Services.AddSingleton(smtpSettings);

configuration = builder.Configuration;

//builder.Services.AddAuthentication().AddGoogle(options =>
//{
//    options.ClientId = configuration["Authentication:Google:ClientId"];
//    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
//});
builder.Services.AddSession();
builder.Services.AddRazorPages();
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        if (context.Database.IsSqlServer())
        {
            context.Database.Migrate();
        }

        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();


        await ApplicationDbContextSeed.SeedDefaultUsersAndRolesAsync(userManager, roleManager, context);
        await ApplicationDbContextSeed.SeedDefaultLocalizationData(context);
        await ApplicationDbContextSeed.SeedDefaultReservationStatuses(context);
        await ApplicationDbContextSeed.SeedDefaultOffers(context);
        await ApplicationDbContextSeed.SeedDefaultPaymentMethods(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogError(ex, "An error occurred while migrating or seeding the database.");

        throw;
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

try
{
    Log.Information("Starting web host");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}