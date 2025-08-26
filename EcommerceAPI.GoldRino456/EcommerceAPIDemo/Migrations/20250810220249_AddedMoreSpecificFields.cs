using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceAPIDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreSpecificFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRefund",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LastFourDigitsOfPaymentCard",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionLastUpdatedDate",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "creditCardType",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRefund",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "LastFourDigitsOfPaymentCard",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TransactionLastUpdatedDate",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "creditCardType",
                table: "Sales");
        }
    }
}
