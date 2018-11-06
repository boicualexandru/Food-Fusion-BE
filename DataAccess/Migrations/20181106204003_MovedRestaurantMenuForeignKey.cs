using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class MovedRestaurantMenuForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Menus_MenuId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_MenuId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MenuId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                schema: "dbo",
                table: "Menus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_RestaurantId",
                schema: "dbo",
                table: "Menus",
                column: "RestaurantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Restaurants_RestaurantId",
                schema: "dbo",
                table: "Menus",
                column: "RestaurantId",
                principalSchema: "dbo",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Restaurants_RestaurantId",
                schema: "dbo",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_RestaurantId",
                schema: "dbo",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                schema: "dbo",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                schema: "dbo",
                table: "Restaurants",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_MenuId",
                schema: "dbo",
                table: "Restaurants",
                column: "MenuId",
                unique: true,
                filter: "[MenuId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Menus_MenuId",
                schema: "dbo",
                table: "Restaurants",
                column: "MenuId",
                principalSchema: "dbo",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
