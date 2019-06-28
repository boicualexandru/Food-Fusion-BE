using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddPaidFieldForReservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                schema: "dbo",
                table: "Reservations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                schema: "dbo",
                table: "HotelRoomReservations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                schema: "dbo",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Paid",
                schema: "dbo",
                table: "HotelRoomReservations");
        }
    }
}
