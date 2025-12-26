using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JafnaEcommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class adddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 28, 10, 46, 27, 110, DateTimeKind.Utc).AddTicks(2478));

            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2025, 11, 28, 10, 46, 27, 110, DateTimeKind.Utc).AddTicks(2482));

            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2025, 11, 28, 10, 46, 27, 110, DateTimeKind.Utc).AddTicks(2484));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 28, 10, 46, 27, 110, DateTimeKind.Utc).AddTicks(2764));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2025, 11, 28, 10, 46, 27, 110, DateTimeKind.Utc).AddTicks(2768));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2025, 11, 28, 10, 46, 27, 110, DateTimeKind.Utc).AddTicks(2770));

            migrationBuilder.UpdateData(
                table: "sale",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 28, 10, 46, 27, 110, DateTimeKind.Utc).AddTicks(2799));

            migrationBuilder.UpdateData(
                table: "sale_details",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 11, 28, 10, 46, 27, 110, DateTimeKind.Utc).AddTicks(2831));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
