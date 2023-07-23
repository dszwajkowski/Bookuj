using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Infrastructure.Persistence.Data.Migrations
{
    public partial class AddStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferFromStoredProcedure");

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE sp_GetOffersWithFilters (
	@DateFrom datetime2 = null,
	@DateTo datetime2 = null,
	@CityID integer = null,
	@LodgingType integer = null,
	@PersonCount integer = null,
	@BedCount integer = null,
	@RoomCount integer = null,
	@PriceMin real = null,
	@PriceMax real = null,
	@SizeMin float = null,
	@SizeMax float = null,
	@AuthorID nvarchar(450) = null,
	@FilterBlockedUsers bit = 1,
	@FilterArchivedOffers bit = 1)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT o.ID, o.Title, o.Description, o.DateCreated, o.DateUpdated, o.LodgingType, 
		o.AddressLine, o.CityID, o.PostalCode, o.AuthorId, u.UserName AS Username, c.Name AS CityName, 
		op.Path, op.FileName, AVG(CAST(oo.Rating AS FLOAT)) AS Rating, COUNT(DISTINCT oo.ID) AS OpinionCount 
	FROM Offer o
	LEFT JOIN LodgingOption lo on lo.OfferID = o.ID
	LEFT JOIN Reservation r on r.LodgingOptionID = lo.ID
	LEFT JOIN AspNetUsers u on u.Id = o.AuthorId
	LEFT JOIN City c on c.ID = o.CityID
	LEFT JOIN OfferPhotos op on op.OfferID = o.ID
		AND op.ID = (SELECT TOP(1) opp.ID
			FROM OfferPhotos opp
			WHERE opp.OfferID = o.ID)
	LEFT JOIN OfferOpinion oo on oo.OfferID = o.ID
	WHERE (o.CityID = @CityID or @CityID is null)
		AND (o.AuthorId = @AuthorID or @AuthorID is null)
		AND (o.LodgingType = @LodgingType or @LodgingType is null)
		AND (o.Archived = 0 and @FilterArchivedOffers = 1)
		AND (lo.PersonCount = @PersonCount or @PersonCount is null)
		AND (lo.BedCount = @BedCount or @BedCount is null)
		AND (lo.RoomCount = @RoomCount or @RoomCount is null)
		AND (lo.PersonCount = @PersonCount or @PersonCount is null)
		AND (lo.Price >= @PriceMin or @PriceMin is null)
		AND (lo.Price <= @PriceMax or @PriceMax is null)
		AND (lo.Size >= @SizeMin or @SizeMin is null)
		AND (lo.Size <= @SizeMax or @SizeMax is null)
		AND ((u.LockUntil > CURRENT_TIMESTAMP or u.LockUntil is null)
			and @FilterBlockedUsers = 1)
		AND NOT EXISTS (
				SELECT TOP(1) 1
				FROM Reservation rr
				WHERE rr.LodgingOptionID = lo.ID
					AND rr.StatusID IN (2, 3)
					AND rr.DateFrom < @DateTo
					AND rr.DateTo > @DateFrom)
	GROUP BY o.ID, o.Title, o.Description, o.DateCreated, o.DateUpdated, o.LodgingType, 
		o.AddressLine, o.CityID, o.PostalCode, o.AuthorId, u.UserName, c.Name, op.Path, op.FileName
END"
				);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfferFromStoredProcedure",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LodgingType = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CityID = table.Column<int>(type: "int", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Rating = table.Column<double>(type: "float", nullable: true),
					OpinionCount = table.Column<int>(type: "int", nullable: false)
				},
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferFromStoredProcedure", x => x.ID);
                });
        }
    }
}
