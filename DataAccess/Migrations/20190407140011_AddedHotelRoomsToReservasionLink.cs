using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddedHotelRoomsToReservasionLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelRoomReservations_HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations",
                column: "HotelRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelRoomReservations_HotelRooms_HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations",
                column: "HotelRoomId",
                principalSchema: "dbo",
                principalTable: "HotelRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelRoomReservations_HotelRooms_HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations");

            migrationBuilder.DropIndex(
                name: "IX_HotelRoomReservations_HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations");

            migrationBuilder.DropColumn(
                name: "HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations");
        }
    }
}
