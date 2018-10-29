using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Restrict_CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedTables_RestaurantTables_RestaurantTableId",
                schema: "dbo",
                table: "ReservedTables");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantEmployees_Users_UserId",
                schema: "dbo",
                table: "RestaurantEmployees");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedTables_RestaurantTables_RestaurantTableId",
                schema: "dbo",
                table: "ReservedTables",
                column: "RestaurantTableId",
                principalSchema: "dbo",
                principalTable: "RestaurantTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantEmployees_Users_UserId",
                schema: "dbo",
                table: "RestaurantEmployees",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedTables_RestaurantTables_RestaurantTableId",
                schema: "dbo",
                table: "ReservedTables");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantEmployees_Users_UserId",
                schema: "dbo",
                table: "RestaurantEmployees");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedTables_RestaurantTables_RestaurantTableId",
                schema: "dbo",
                table: "ReservedTables",
                column: "RestaurantTableId",
                principalSchema: "dbo",
                principalTable: "RestaurantTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantEmployees_Users_UserId",
                schema: "dbo",
                table: "RestaurantEmployees",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
