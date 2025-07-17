using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Exercise_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Comments", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, "Morning workout session.", new DateTime(2025, 7, 15, 8, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 15, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Afternoon workout session.", new DateTime(2025, 7, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 16, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Evening workout session.", new DateTime(2025, 7, 17, 21, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 17, 21, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercises");
        }
    }
}
