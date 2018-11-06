using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class MovedRestaurantMapForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_RestaurantMaps_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "RestaurantMapId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                schema: "dbo",
                table: "RestaurantMaps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantMaps_RestaurantId",
                schema: "dbo",
                table: "RestaurantMaps",
                column: "RestaurantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantMaps_Restaurants_RestaurantId",
                schema: "dbo",
                table: "RestaurantMaps",
                column: "RestaurantId",
                principalSchema: "dbo",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantMaps_Restaurants_RestaurantId",
                schema: "dbo",
                table: "RestaurantMaps");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantMaps_RestaurantId",
                schema: "dbo",
                table: "RestaurantMaps");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                schema: "dbo",
                table: "RestaurantMaps");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantMapId",
                schema: "dbo",
                table: "Restaurants",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants",
                column: "RestaurantMapId",
                unique: true,
                filter: "[RestaurantMapId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_RestaurantMaps_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants",
                column: "RestaurantMapId",
                principalSchema: "dbo",
                principalTable: "RestaurantMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
