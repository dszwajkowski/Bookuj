using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Infrastructure.Persistence.Data.Migrations
{
    public partial class UpdateCartSchemaRemoveConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_AspNetUsers_ID",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Cart_CartID",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_CartID",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "CartID",
                table: "Reservation");

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "Cart",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Cart",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserID",
                table: "Cart",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_AspNetUsers_UserID",
                table: "Cart",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_AspNetUsers_UserID",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_UserID",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Cart");

            migrationBuilder.AddColumn<string>(
                name: "CartID",
                table: "Reservation",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "Cart",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart",
                table: "Cart",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_CartID",
                table: "Reservation",
                column: "CartID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_AspNetUsers_ID",
                table: "Cart",
                column: "ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Cart_CartID",
                table: "Reservation",
                column: "CartID",
                principalTable: "Cart",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
