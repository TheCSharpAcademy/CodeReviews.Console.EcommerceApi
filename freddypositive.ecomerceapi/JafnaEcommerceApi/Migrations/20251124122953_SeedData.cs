using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JafnaEcommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "product_name",
                table: "sale_details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "id", "created_date", "is_deleted", "name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 24, 12, 29, 52, 483, DateTimeKind.Utc).AddTicks(9784), false, "Fish" },
                    { 2, new DateTime(2025, 11, 24, 12, 29, 52, 483, DateTimeKind.Utc).AddTicks(9788), false, "Plants" },
                    { 3, new DateTime(2025, 11, 24, 12, 29, 52, 483, DateTimeKind.Utc).AddTicks(9790), false, "Aquarium Supplies" }
                });

            migrationBuilder.InsertData(
                table: "sale",
                columns: new[] { "id", "created_date", "total_price" },
                values: new object[] { 1, new DateTime(2025, 11, 24, 12, 29, 52, 484, DateTimeKind.Utc).AddTicks(153), 200m });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "created_date", "description", "image", "is_deleted", "name", "price" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 11, 24, 12, 29, 52, 484, DateTimeKind.Utc).AddTicks(114), "Beautiful hardy goldfish", "goldfish.jpg", false, "Goldfish", 100m },
                    { 2, 2, new DateTime(2025, 11, 24, 12, 29, 52, 484, DateTimeKind.Utc).AddTicks(118), "Popular aquarium plant", "amazonsword.jpg", false, "Amazon Sword", 150m },
                    { 3, 3, new DateTime(2025, 11, 24, 12, 29, 52, 484, DateTimeKind.Utc).AddTicks(120), "Small internal filter", "filter.jpg", false, "Internal Filter", 500m }
                });

            migrationBuilder.InsertData(
                table: "sale_details",
                columns: new[] { "id", "created_date", "product_id", "product_name", "product_price", "product_quantity", "product_total_price", "sale_id" },
                values: new object[] { 1, new DateTime(2025, 11, 24, 12, 29, 52, 484, DateTimeKind.Utc).AddTicks(183), 1, "Goldfish", 100m, 2, 200m, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "sale_details",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "sale",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "product_name",
                table: "sale_details");
        }
    }
}
