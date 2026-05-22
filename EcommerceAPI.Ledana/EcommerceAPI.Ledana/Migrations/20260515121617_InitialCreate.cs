using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceAPI.Ledana.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleProduct",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    SalesId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPriceAtSale = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "([Quantity] * [UnitPriceAtSale]) * (1 - [Discount])", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleProduct", x => new { x.SalesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_SaleProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleProduct_Sales_SalesId",
                        column: x => x.SalesId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Peripherals" },
                    { 2, "Monitors and Displays" },
                    { 3, "Computers and Laptops" },
                    { 4, "Components" },
                    { 5, "Audio and Headsets" },
                    { 6, "Video and Cameras" }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "Date" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 30, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2026, 5, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2026, 5, 2, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2026, 5, 3, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2026, 5, 4, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Wireless Mouse", 9.99m },
                    { 2, 1, "Nose Cancelling HeadPhones", 29.99m },
                    { 3, 1, "Silent Keyboard", 19.99m },
                    { 4, 2, "Curved Monitor", 599.99m },
                    { 5, 2, "All Surface Projector", 129.99m },
                    { 6, 2, "Wide 4K Monitor", 219.99m },
                    { 7, 3, "Gaming Laptop", 899.99m },
                    { 8, 3, "Workstation PC", 1029.99m },
                    { 9, 4, "CPU", 299.99m },
                    { 10, 4, "GPU", 109.99m },
                    { 11, 4, "RAM", 489.99m },
                    { 12, 4, "SSD", 119.99m },
                    { 13, 5, "Gaming Headset", 89.99m },
                    { 14, 5, "Studio Mic", 199.99m },
                    { 15, 6, "Webcam", 215.99m },
                    { 16, 6, "Capture Card", 708.99m },
                    { 17, 6, "360 Camera", 4009.99m }
                });

            migrationBuilder.InsertData(
                table: "SaleProduct",
                columns: new[] { "ProductsId", "SalesId", "Discount", "Quantity", "UnitPriceAtSale" },
                values: new object[,]
                {
                    { 1, 1, 0.10m, 2, 9.99m },
                    { 3, 1, 0.05m, 2, 19.99m },
                    { 1, 2, 0m, 1, 9.99m },
                    { 4, 2, 0.20m, 2, 599.99m },
                    { 5, 2, 0.20m, 2, 129.99m },
                    { 5, 3, 0.10m, 1, 129.99m },
                    { 7, 3, 0m, 1, 899.99m },
                    { 8, 3, 0.05m, 1, 1029.99m },
                    { 5, 4, 0.20m, 2, 129.99m },
                    { 8, 4, 0.05m, 1, 1029.99m },
                    { 9, 4, 0m, 1, 299.99m },
                    { 15, 4, 0.10m, 1, 215.99m },
                    { 17, 4, 0.05m, 1, 4009.99m },
                    { 1, 5, 0.20m, 2, 9.99m },
                    { 11, 5, 0.10m, 1, 489.99m },
                    { 12, 5, 0m, 1, 119.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProduct_ProductsId",
                table: "SaleProduct",
                column: "ProductsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleProduct");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
