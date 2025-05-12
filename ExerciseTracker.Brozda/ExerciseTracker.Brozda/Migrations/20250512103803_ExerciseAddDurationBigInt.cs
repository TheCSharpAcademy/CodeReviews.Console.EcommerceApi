using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseTracker.Brozda.Migrations
{
    /// <inheritdoc />
    public partial class ExerciseAddDurationBigInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "Exercises",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Exercises");
        }
    }
}
