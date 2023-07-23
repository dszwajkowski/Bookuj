
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Persistence.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUsersAndRolesAsync(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IApplicationDataContext context)
        {
            var adminRole = new IdentityRole("Admin");
            var sellerRole = new IdentityRole("Sprzedawca");

            var roles = new List<IdentityRole>()
            {
                adminRole,
                sellerRole,
                //new IdentityRole("Użytkownicy")
            };

            foreach (var role in roles)
            {
                if (roleManager.Roles.All(r => r.Name != role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            int? defaultAvatarID = await SeedDefaultUserAvatar(context);
            ArgumentNullException.ThrowIfNull(defaultAvatarID);

            var adminUser = new User 
            { 
                UserName = "Admin", 
                Email = "admin@mail.com", 
                EmailConfirmed = true,
                AvatarID = defaultAvatarID
            };
            var testUser = new User 
            { 
                UserName = "JanTestowy", 
                Email = "jantestowy@mail.com", 
                EmailConfirmed = true,
                AvatarID = defaultAvatarID
            };

            if (userManager.Users.All(u => u.UserName != adminUser.UserName))
            {
                await userManager.CreateAsync(adminUser, "Qwerty123!");
                await userManager.AddToRolesAsync(adminUser, new[] { adminRole.Name });
                context.Cart.Add(new Cart { UserID = adminUser.Id });
            }

            if (userManager.Users.All(u => u.UserName != testUser.UserName))
            {
                await userManager.CreateAsync(testUser, "Qwerty123!");
                await userManager.AddToRolesAsync(testUser, new[] { sellerRole.Name });
                context.Cart.Add(new Cart { UserID = testUser.Id });
            }

            await context.SaveChangesAsync(CancellationToken.None);
        }

        public static async Task<int?> SeedDefaultUserAvatar(IApplicationDataContext context)
        {
            var defaultAvatar = context.UserAvatar
                .Where(x => x.FileName.StartsWith("default"))
                .FirstOrDefault();

            if (defaultAvatar is null)
            {
                defaultAvatar = new UserAvatar
                {
                    Path = @"\files\images\profiles\",
                    FileName = "default.png"
                };
                context.UserAvatar.Add(defaultAvatar);
                await context.SaveChangesAsync(CancellationToken.None);
            }

            return defaultAvatar?.ID;
        }

        public static async Task SeedDefaultLocalizationData(IApplicationDataContext context)
        {
            if (!context.City.Any() && !context.Region.Any() && !context.City.Any())
            {
                Country pl = new() { Name = "Polska", Code = "PL" };
                
                if (context.Region.All(r => r.Name != pl.Name))
                {
                    context.Country.Add(pl);
                }
                

                Region doln = new() { Name = "Dolnośląskie", Country = pl };
                Region kuja = new() { Name = "Kujawsko-pomorskie", Country = pl };
                Region lubu = new() { Name = "Lubuskie", Country = pl };
                Region lube = new() { Name = "Lubelskie", Country = pl };
                Region lod = new() { Name = "Łódźkie", Country = pl };
                Region malo = new() { Name = "Małopolskie", Country = pl };
                Region mazo = new() { Name = "Mazowiecki", Country = pl };
                Region opol = new() { Name = "Opolskie", Country = pl };
                Region podk = new() { Name = "Podkarpackie", Country = pl };
                Region podl = new() { Name = "Podlaskie", Country = pl };
                Region slas = new() { Name = "Śląskie", Country = pl };
                Region swiet = new() { Name = "Świętokrzyskie", Country = pl };
                Region warm = new() { Name = "Warmińsko-mazurskie", Country = pl };
                Region wiel = new() { Name = "Wielkopolskie", Country = pl };
                Region zachp = new() { Name = "Zachodniopomorskie", Country = pl };

                List<Region> regions = new() { doln, kuja, lubu, lube, lod, malo, mazo, opol, podk, podl, slas, swiet, warm, wiel, zachp };

                foreach (var region in regions)
                {
                    if (context.Region.All(r => r.Name != region.Name))
                    {
                        context.Region.Add(region);
                    }
                }

                City wroc = new() { Name = "Wrocław", Region = doln };
                City bydg = new() { Name = "Bydgoszcz", Region = kuja };
                City lubl = new() { Name = "Lublin", Region = lubu };
                City gorz = new() { Name = "Gorzów Wielkopolski", Region = lube };
                City lodz = new() { Name = "Łódź", Region = lod };
                City krak = new() { Name = "Kraków", Region = malo };
                City wars = new() { Name = "Warszawa", Region = mazo };
                City opole = new() { Name = "Opole", Region = opol };
                City rzes = new() { Name = "Rzeszów", Region = podk };
                City bial = new() { Name = "Białystok", Region = podl };
                City kato = new() { Name = "Katowice", Region = slas };
                City kiel = new() { Name = "Kielce", Region = swiet };
                City olsz = new() { Name = "Olszyt", Region = warm };
                City pozn = new() { Name = "Poznań", Region = wiel };
                City szcz = new() { Name = "Szczecin", Region = zachp };

                List<City> cities = new () { wroc, bydg, lubl, gorz, lodz, krak, wars, opole, rzes, bial, kato, kiel, olsz, pozn };

                foreach(var city in cities)
                {
                    if (context.City.All(r => r.Name != city.Name))
                    {
                        context.City.Add(city);
                    }
                }

                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        public static async Task SeedDefaultOffers(IApplicationDataContext context)
        {
            if (!context.Offer.Any())
            {
                var cityWarsaw = await context.City
                    .Where(x => x.Name == "Warszawa")
                    .SingleAsync();

                var user1 = await context.Users
                    .Where(x => x.UserName == "Admin")
                    .SingleAsync();
                var user2 = await context.Users
                    .Where(x => x.UserName == "JanTestowy")
                    .SingleAsync();


                var offer1 = new Offer
                {
                    Title = "Hot Palace",
                    Description = "Renomowany hotel Hot Palace działa od 2000 roku. Wyróżnia się on świetną lokalizacją przy Trakcie Królewskim. Dzięki czemu tylko kilka minut zajmie nam dotarcie do XIX-wiecznego gmachu Opery Narodowej. Z okien budynku roztaczają się widoki na panoramę miasta. Obiekt ten jest bardzo nowoczesny i posiada mnóstwo udogodnień dla gości. Wyróżnia go klasyczny styl współgrający ze  współczesnym wystrojem wnętrz. Główny hol zdobią dzieła polskich i zagranicznych malarzy. Obiekt Hot Palace oferuje całodobową możliwość skorzystania z pomocy naszych pracowników.  Wszystkie pokoje i apartamenty są klimatyzowane i urządzone w eleganckim stylu. W większości z nich zapewniono w telewizor z dostępem do kanałów satelitarnych. W pozostałych goście mogą korzystać z wieży stereo. Pokoje i apartamenty zdobią satynowe zasłony oraz eleganckie elementy dekoracyjne. Łazienka dysponuje wanną z  widokiem na miasto. Cena zakwaterowania w apartamentach obejmuje również wizytę w SPA i masaż.",
                    AddressLine = "Powietrzna 12",
                    CityID = cityWarsaw.ID,
                    PostalCode = "00-127",
                    AuthorId = user1.Id
                };
                context.Offer.Add(offer1);
                
                var offer2 = new Offer
                {
                    Title = "Hotel StarTime",
                    Description = "Pokoje są wszechstronne, a każdy z nich ma dostęp do własnej łazienki. Każdy pokój ma dostęp do balkonu, gdzie goście mają możliwość wypicia porannej kawy.  Hotel zapewnia nieograniczony dostęp do spa w których goście znajdą sauny, łaźnie parowe, kryty basen i siłownię. W pokojach znajdują się łóżka wykonane z drewna dębowego i najwyższej klasy materace. W restauracji StarTime każdego ranka serwowane jest śniadanie do wyboru z karty. Na każdy dzień mamy przygotowane specjalne danie i drink dnia. Lokal ten oferuje również posiłki w innych porach dnia, zarówno dla wegetarian, wegan oraz alergików zachodnim skrzydle hotelu znajduje się mały bar z napojami alkoholowymi, czekoladą i herbatą, W sąsiedniej Sali będą mogli Państwo delektować się pysznymi ciastami i wypiekami. ",
                    AddressLine = "Piaseczna 33",
                    CityID = cityWarsaw.ID,
                    PostalCode = "00-128",
                    AuthorId = user2.Id
                };
                context.Offer.Add(offer2);

                var offer3 = new Offer
                {
                    Title = "Parlor",
                    Description = "Ten obiekt jest położony 5 minut spacerem od centrum. Parlor jest, położone blisko Wisły. Nasz hotel, oferuje wspólny salon, zakwaterowanie w pokojach dla alergików, bezpłatne WiFi oraz ogród.Dzięki lokalizacji mamy świetny widok z każdego wynajmowanego okna. Oferta obiektu obejmuje obsługę pokoju. Na miejscu znajduje się również plac zabaw. Oferta ośrodka wypoczynkowego obejmuje również pokoje rodzinne.  W każdym pokoju w obiekcie znajduje się szafa, telewizor z płaskim ekranem oraz prywatna łazienka. Pokoje we wschodnim skrzydle są wyposażone w ekspres do kawy. Pościel i ręczniki są zapewnione. Każdy pokój wyposażono w czujnik dymu. Wybrane opcje zakwaterowania mają również patio, a z innych roztacza się widok na ogród. Każdego ranka na miejscu serwowane jest śniadanie w postaci szwedzkiego stołu. W pobliżu naszego obiektu znajduje się mnóstwo restauracji i knajpek. ",
                    AddressLine = "Wiślana 16",
                    CityID = cityWarsaw.ID,
                    PostalCode = "00-150",
                    AuthorId = user2.Id
                };
                context.Offer.Add(offer3);
                await context.SaveChangesAsync(CancellationToken.None);

                context.LodgingOption.Add(
                    new LodgingOption
                    {
                        Price = 140,
                        PersonCount = 3,
                        RoomCount = 2,
                        BedCount = 2,
                        Size = 45,
                        OfferID = offer1.ID
                    });
                context.LodgingOption.Add(
                    new LodgingOption
                    {
                        Price = 135,
                        PersonCount = 4,
                        RoomCount = 2,
                        BedCount = 2,
                        Size = 45,
                        OfferID = offer1.ID
                    });
                context.LodgingOption.Add(
                    new LodgingOption
                    {
                        Price = 135,
                        PersonCount = 4,
                        RoomCount = 2,
                        BedCount = 2,
                        Size = 45,
                        OfferID = offer1.ID
                    });

                context.LodgingOption.Add(
                    new LodgingOption
                    {
                        Price = 140,
                        PersonCount = 3,
                        RoomCount = 2,
                        BedCount = 2,
                        Size = 45,
                        OfferID = offer2.ID
                    });
                context.LodgingOption.Add(
                    new LodgingOption
                    {
                        Price = 135,
                        PersonCount = 4,
                        RoomCount = 2,
                        BedCount = 2,
                        Size = 45,
                        OfferID = offer2.ID
                    });
                context.LodgingOption.Add(
                    new LodgingOption
                    {
                        Price = 135,
                        PersonCount = 4,
                        RoomCount = 2,
                        BedCount = 2,
                        Size = 45,
                        OfferID = offer2.ID
                    });

                context.LodgingOption.Add(
                    new LodgingOption
                    {
                        Price = 140,
                        PersonCount = 3,
                        RoomCount = 2,
                        BedCount = 2,
                        Size = 45,
                        OfferID = offer3.ID
                    });
                context.LodgingOption.Add(
                    new LodgingOption
                    {
                        Price = 135,
                        PersonCount = 4,
                        RoomCount = 2,
                        BedCount = 2,
                        Size = 45,
                        OfferID = offer3.ID
                    });
                context.LodgingOption.Add(
                    new LodgingOption
                    {
                        Price = 135,
                        PersonCount = 4,
                        RoomCount = 2,
                        BedCount = 2,
                        Size = 45,
                        OfferID = offer3.ID
                    });

                context.OfferPhotos.Add(new OfferPhoto
                {
                    Path = @"\files\images\offer\exampleOffer1",
                    FileName = "1.jpg",
                    OfferID = offer1.ID
                });
                context.OfferPhotos.Add(new OfferPhoto
                {
                    Path = @"\files\images\offer\exampleOffer1",
                    FileName = "2.jpg",
                    OfferID = offer1.ID
                });
                context.OfferPhotos.Add(new OfferPhoto
                {
                    Path = @"\files\images\offer\exampleOffer1",
                    FileName = "3.jpg",
                    OfferID = offer1.ID
                });
                context.OfferPhotos.Add(new OfferPhoto
                {
                    Path = @"\files\images\offer\exampleOffer1",
                    FileName = "4.jpg",
                    OfferID = offer1.ID
                });

                context.OfferPhotos.Add(new OfferPhoto
                {
                    Path = @"\files\images\offer\exampleOffer2",
                    FileName = "1.jpg",
                    OfferID = offer2.ID
                });
                context.OfferPhotos.Add(new OfferPhoto
                {
                    Path = @"\files\images\offer\exampleOffer2",
                    FileName = "2.jpg",
                    OfferID = offer2.ID
                });
                context.OfferPhotos.Add(new OfferPhoto
                {
                    Path = @"\files\images\offer\exampleOffer2",
                    FileName = "3.jpg",
                    OfferID = offer2.ID
                });

                context.OfferPhotos.Add(new OfferPhoto
                {
                    Path = @"\files\images\offer\exampleOffer3",
                    FileName = "1.jpg",
                    OfferID = offer3.ID
                });

                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        public async static Task SeedDefaultReservationStatuses(IApplicationDataContext context)
        {
            if (!context.ReservationStatus.Any())
            {
                context.ReservationStatus.Add(new ReservationStatus
                {
                    Name = "Niepotwierdzona"
                });
                context.ReservationStatus.Add(new ReservationStatus
                {
                    Name = "Oczekuje na potwierdzenie"
                });
                context.ReservationStatus.Add(new ReservationStatus
                {
                    Name = "Potwierdzona"
                });
                context.ReservationStatus.Add(new ReservationStatus
                {
                    Name = "Anulowana"
                });

                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        public async static Task SeedDefaultPaymentMethods(IApplicationDataContext context)
        {
            if (!context.PaymentMethod.Any())
            {
                context.PaymentMethod.Add(new PaymentMethod
                {
                    Name = "Płatność kartą na miejscu."
                });
                context.PaymentMethod.Add(new PaymentMethod
                {
                    Name = "Płatność gotówką na miejscu."
                });
                context.PaymentMethod.Add(new PaymentMethod
                {
                    Name = "Płatność kartą online."
                });

                await context.SaveChangesAsync(CancellationToken.None);
            }
        }
    }
}
