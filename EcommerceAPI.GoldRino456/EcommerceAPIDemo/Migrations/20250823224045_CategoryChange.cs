using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceAPIDemo.Migrations
{
    /// <inheritdoc />
    public partial class CategoryChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameCategoryGameProduct");

            migrationBuilder.AddColumn<int>(
                name: "GameProductId",
                table: "GameCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameCategories_GameProductId",
                table: "GameCategories",
                column: "GameProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameCategories_GameProducts_GameProductId",
                table: "GameCategories",
                column: "GameProductId",
                principalTable: "GameProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCategories_GameProducts_GameProductId",
                table: "GameCategories");

            migrationBuilder.DropIndex(
                name: "IX_GameCategories_GameProductId",
                table: "GameCategories");

            migrationBuilder.DropColumn(
                name: "GameProductId",
                table: "GameCategories");

            migrationBuilder.CreateTable(
                name: "GameCategoryGameProduct",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    GamesInCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCategoryGameProduct", x => new { x.CategoriesId, x.GamesInCategoryId });
                    table.ForeignKey(
                        name: "FK_GameCategoryGameProduct_GameCategories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "GameCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameCategoryGameProduct_GameProducts_GamesInCategoryId",
                        column: x => x.GamesInCategoryId,
                        principalTable: "GameProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameCategoryGameProduct_GamesInCategoryId",
                table: "GameCategoryGameProduct",
                column: "GamesInCategoryId");
        }
    }
}
