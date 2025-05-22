using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseTracker.KamilKolanowski.Migrations
{
    /// <inheritdoc />
    public partial class extendSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExerciseType",
                schema: "TCSA",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExerciseType",
                schema: "TCSA",
                table: "Exercises");
        }
    }
}
