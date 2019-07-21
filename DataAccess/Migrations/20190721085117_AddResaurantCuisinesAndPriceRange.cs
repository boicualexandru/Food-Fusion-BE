using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddResaurantCuisinesAndPriceRange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceRange",
                schema: "dbo",
                table: "Restaurants",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cuisine",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuisine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantCuisines",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RestaurantId = table.Column<int>(nullable: false),
                    CuisineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantCuisines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantCuisines_Cuisine_CuisineId",
                        column: x => x.CuisineId,
                        principalSchema: "dbo",
                        principalTable: "Cuisine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantCuisines_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "dbo",
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantCuisines_CuisineId",
                schema: "dbo",
                table: "RestaurantCuisines",
                column: "CuisineId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantCuisines_RestaurantId",
                schema: "dbo",
                table: "RestaurantCuisines",
                column: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantCuisines",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Cuisine",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "PriceRange",
                schema: "dbo",
                table: "Restaurants");
        }
    }
}
