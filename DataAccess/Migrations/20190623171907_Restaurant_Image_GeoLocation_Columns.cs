using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Restaurant_Image_GeoLocation_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GeoLatitude",
                schema: "dbo",
                table: "Restaurants",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "GeoLongitude",
                schema: "dbo",
                table: "Restaurants",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "dbo",
                table: "Restaurants",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeoLatitude",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "GeoLongitude",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "dbo",
                table: "Restaurants");
        }
    }
}
