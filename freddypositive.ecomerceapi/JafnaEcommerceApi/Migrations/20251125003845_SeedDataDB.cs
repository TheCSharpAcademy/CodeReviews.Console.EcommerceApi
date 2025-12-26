using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JafnaEcommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 25, 0, 38, 45, 372, DateTimeKind.Utc).AddTicks(6892));

            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2025, 11, 25, 0, 38, 45, 372, DateTimeKind.Utc).AddTicks(6894));

            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2025, 11, 25, 0, 38, 45, 372, DateTimeKind.Utc).AddTicks(6895));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 25, 0, 38, 45, 372, DateTimeKind.Utc).AddTicks(7055));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2025, 11, 25, 0, 38, 45, 372, DateTimeKind.Utc).AddTicks(7057));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2025, 11, 25, 0, 38, 45, 372, DateTimeKind.Utc).AddTicks(7058));

            migrationBuilder.UpdateData(
                table: "sale",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 25, 0, 38, 45, 372, DateTimeKind.Utc).AddTicks(7082));

            migrationBuilder.UpdateData(
                table: "sale_details",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 25, 0, 38, 45, 372, DateTimeKind.Utc).AddTicks(7113));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 24, 12, 29, 52, 483, DateTimeKind.Utc).AddTicks(9784));

            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2025, 11, 24, 12, 29, 52, 483, DateTimeKind.Utc).AddTicks(9788));

            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2025, 11, 24, 12, 29, 52, 483, DateTimeKind.Utc).AddTicks(9790));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 24, 12, 29, 52, 484, DateTimeKind.Utc).AddTicks(114));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2025, 11, 24, 12, 29, 52, 484, DateTimeKind.Utc).AddTicks(118));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2025, 11, 24, 12, 29, 52, 484, DateTimeKind.Utc).AddTicks(120));

            migrationBuilder.UpdateData(
                table: "sale",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 24, 12, 29, 52, 484, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "sale_details",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 24, 12, 29, 52, 484, DateTimeKind.Utc).AddTicks(183));
        }
    }
}
