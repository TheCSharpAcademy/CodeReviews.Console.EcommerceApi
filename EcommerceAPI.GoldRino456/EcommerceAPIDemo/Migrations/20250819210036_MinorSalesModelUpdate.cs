using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceAPIDemo.Migrations
{
    /// <inheritdoc />
    public partial class MinorSalesModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ActualTransactionValue",
                table: "Sales",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPartialRefund",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualTransactionValue",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IsPartialRefund",
                table: "Sales");
        }
    }
}
