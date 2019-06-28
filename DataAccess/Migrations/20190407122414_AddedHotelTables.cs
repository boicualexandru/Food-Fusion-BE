using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddedHotelTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelFeatures",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Category = table.Column<string>(maxLength: 200, nullable: false),
                    Icon = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelRoomReservations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoomId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    GuestsCount = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRoomReservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelRooms",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Floor = table.Column<int>(nullable: false),
                    Beds = table.Column<int>(nullable: false),
                    MaxGuests = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelRoomFeatures",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoomId = table.Column<int>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRoomFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelRoomFeatures_HotelFeatures_FeatureId",
                        column: x => x.FeatureId,
                        principalSchema: "dbo",
                        principalTable: "HotelFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelRoomFeatures_HotelRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "dbo",
                        principalTable: "HotelRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelRoomFeatures_FeatureId",
                schema: "dbo",
                table: "HotelRoomFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelRoomFeatures_RoomId",
                schema: "dbo",
                table: "HotelRoomFeatures",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelRoomFeatures",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "HotelRoomReservations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "HotelFeatures",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "HotelRooms",
                schema: "dbo");
        }
    }
}
