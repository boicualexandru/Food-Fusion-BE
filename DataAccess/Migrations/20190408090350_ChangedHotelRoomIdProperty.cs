using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class ChangedHotelRoomIdProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelRoomReservations_HotelRooms_HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations");

            migrationBuilder.DropColumn(
                name: "RoomId",
                schema: "dbo",
                table: "HotelRoomReservations");

            migrationBuilder.AlterColumn<int>(
                name: "HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelRoomReservations_UserId",
                schema: "dbo",
                table: "HotelRoomReservations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelRoomReservations_HotelRooms_HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations",
                column: "HotelRoomId",
                principalSchema: "dbo",
                principalTable: "HotelRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelRoomReservations_Users_UserId",
                schema: "dbo",
                table: "HotelRoomReservations",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelRoomReservations_HotelRooms_HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelRoomReservations_Users_UserId",
                schema: "dbo",
                table: "HotelRoomReservations");

            migrationBuilder.DropIndex(
                name: "IX_HotelRoomReservations_UserId",
                schema: "dbo",
                table: "HotelRoomReservations");

            migrationBuilder.AlterColumn<int>(
                name: "HotelRoomId",
                schema: "dbo",
                table: "HotelRoomReservations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                schema: "dbo",
                table: "HotelRoomReservations",
                nullable: false,
                defaultValue: 0);

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
    }
}
