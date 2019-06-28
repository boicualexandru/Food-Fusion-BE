using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class RestaurantGeolocationTypeDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "GeoLongitude",
                schema: "dbo",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "GeoLatitude",
                schema: "dbo",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "GeoLongitude",
                schema: "dbo",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "GeoLatitude",
                schema: "dbo",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
