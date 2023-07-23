using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Infrastructure.Persistence.Data.Migrations
{
    public partial class CreateOfferSchema : Migration
    {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Country",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Country", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "LodgingFacilities",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LodgingFacilities", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "Region",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                CountryID = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Region", x => x.ID);
                table.ForeignKey(
                    name: "FK_Region_Country_CountryID",
                    column: x => x.CountryID,
                    principalTable: "Country",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "City",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                RegionID = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_City", x => x.ID);
                table.ForeignKey(
                    name: "FK_City_Region_RegionID",
                    column: x => x.RegionID,
                    principalTable: "Region",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Offer",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                LodgingType = table.Column<int>(type: "int", nullable: false),
                DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                CityID = table.Column<int>(type: "int", nullable: false),
                AddressLine = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Offer", x => x.ID);
                table.ForeignKey(
                    name: "FK_Offer_AspNetUsers_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Offer_City_CityID",
                    column: x => x.CityID,
                    principalTable: "City",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "LodgingOption",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Price = table.Column<float>(type: "real", nullable: false),
                PersonCount = table.Column<int>(type: "int", nullable: false),
                RoomCount = table.Column<int>(type: "int", nullable: false),
                BedCount = table.Column<int>(type: "int", nullable: false),
                Size = table.Column<double>(type: "float", nullable: false),
                OfferID = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LodgingOption", x => x.ID);
                table.ForeignKey(
                    name: "FK_LodgingOption_Offer_OfferID",
                    column: x => x.OfferID,
                    principalTable: "Offer",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "LodgingFacilitiesLodgingOption",
            columns: table => new
            {
                LodgingFacilitiesID = table.Column<int>(type: "int", nullable: false),
                LodgingOptionsID = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LodgingFacilitiesLodgingOption", x => new { x.LodgingFacilitiesID, x.LodgingOptionsID });
                table.ForeignKey(
                    name: "FK_LodgingFacilitiesLodgingOption_LodgingFacilities_LodgingFacilitiesID",
                    column: x => x.LodgingFacilitiesID,
                    principalTable: "LodgingFacilities",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_LodgingFacilitiesLodgingOption_LodgingOption_LodgingOptionsID",
                    column: x => x.LodgingOptionsID,
                    principalTable: "LodgingOption",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_City_RegionID",
            table: "City",
            column: "RegionID");

        migrationBuilder.CreateIndex(
            name: "IX_LodgingFacilitiesLodgingOption_LodgingOptionsID",
            table: "LodgingFacilitiesLodgingOption",
            column: "LodgingOptionsID");

        migrationBuilder.CreateIndex(
            name: "IX_LodgingOption_OfferID",
            table: "LodgingOption",
            column: "OfferID");

        migrationBuilder.CreateIndex(
            name: "IX_Offer_AuthorId",
            table: "Offer",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_Offer_CityID",
            table: "Offer",
            column: "CityID");

        migrationBuilder.CreateIndex(
            name: "IX_Region_CountryID",
            table: "Region",
            column: "CountryID");
        // pozostałe tabele
        // ...
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "LodgingFacilitiesLodgingOption");

        migrationBuilder.DropTable(
            name: "LodgingFacilities");

        migrationBuilder.DropTable(
            name: "LodgingOption");

        migrationBuilder.DropTable(
            name: "Offer");

        migrationBuilder.DropTable(
            name: "City");

        migrationBuilder.DropTable(
            name: "Region");

        migrationBuilder.DropTable(
            name: "Country");

        // pozostałe tabele
        // ...
    }
    }
}
