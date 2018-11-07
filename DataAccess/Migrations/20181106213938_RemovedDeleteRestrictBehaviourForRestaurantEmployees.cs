using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class RemovedDeleteRestrictBehaviourForRestaurantEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantEmployees_Users_UserId",
                schema: "dbo",
                table: "RestaurantEmployees");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantEmployees_Users_UserId",
                schema: "dbo",
                table: "RestaurantEmployees");

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
    }
}
