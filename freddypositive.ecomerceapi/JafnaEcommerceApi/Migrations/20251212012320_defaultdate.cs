using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JafnaEcommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class defaultdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "sale_details",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "sale",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 12, 12, 1, 23, 19, 607, DateTimeKind.Utc).AddTicks(4285));

            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2025, 12, 12, 1, 23, 19, 607, DateTimeKind.Utc).AddTicks(4288));

            migrationBuilder.UpdateData(
                table: "category",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2025, 12, 12, 1, 23, 19, 607, DateTimeKind.Utc).AddTicks(4289));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 12, 12, 1, 23, 19, 607, DateTimeKind.Utc).AddTicks(4398));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2025, 12, 12, 1, 23, 19, 607, DateTimeKind.Utc).AddTicks(4400));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2025, 12, 12, 1, 23, 19, 607, DateTimeKind.Utc).AddTicks(4401));

            migrationBuilder.UpdateData(
                table: "sale",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 12, 12, 1, 23, 19, 607, DateTimeKind.Utc).AddTicks(4425));

            migrationBuilder.UpdateData(
                table: "sale_details",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2025, 12, 12, 1, 23, 19, 607, DateTimeKind.Utc).AddTicks(4454));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "sale_details",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "sale",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

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
    }
}
