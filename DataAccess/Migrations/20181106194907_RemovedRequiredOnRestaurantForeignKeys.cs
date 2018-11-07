using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class RemovedRequiredOnRestaurantForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_ManagerId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Menus_MenuId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_RestaurantMaps_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_MenuId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantMapId",
                schema: "dbo",
                table: "Restaurants",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MenuId",
                schema: "dbo",
                table: "Restaurants",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                schema: "dbo",
                table: "Restaurants",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "dbo",
                table: "Restaurants",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_MenuId",
                schema: "dbo",
                table: "Restaurants",
                column: "MenuId",
                unique: true,
                filter: "[MenuId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants",
                column: "RestaurantMapId",
                unique: true,
                filter: "[RestaurantMapId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_ManagerId",
                schema: "dbo",
                table: "Restaurants",
                column: "ManagerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Menus_MenuId",
                schema: "dbo",
                table: "Restaurants",
                column: "MenuId",
                principalSchema: "dbo",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_ManagerId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Menus_MenuId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_RestaurantMaps_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_MenuId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantMapId",
                schema: "dbo",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MenuId",
                schema: "dbo",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                schema: "dbo",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "dbo",
                table: "Restaurants",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_MenuId",
                schema: "dbo",
                table: "Restaurants",
                column: "MenuId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants",
                column: "RestaurantMapId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_ManagerId",
                schema: "dbo",
                table: "Restaurants",
                column: "ManagerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Menus_MenuId",
                schema: "dbo",
                table: "Restaurants",
                column: "MenuId",
                principalSchema: "dbo",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_RestaurantMaps_RestaurantMapId",
                schema: "dbo",
                table: "Restaurants",
                column: "RestaurantMapId",
                principalSchema: "dbo",
                principalTable: "RestaurantMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
