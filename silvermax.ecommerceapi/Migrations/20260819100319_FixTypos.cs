using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace silvermax.ecommerceapi.Migrations
{
    /// <inheritdoc />
    public partial class FixTypos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "OrderItems",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "OrderItems",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderItems",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderItems",
                newName: "price");
        }
    }
}
