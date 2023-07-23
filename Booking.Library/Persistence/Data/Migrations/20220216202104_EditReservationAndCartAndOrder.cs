using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Infrastructure.Persistence.Data.Migrations
{
    public partial class EditReservationAndCartAndOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Cart");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Reservation",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "LodgingOption",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<string>(
                name: "SessionID",
                table: "Cart",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "SessionID",
                table: "Cart");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "LodgingOption",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Cart",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
