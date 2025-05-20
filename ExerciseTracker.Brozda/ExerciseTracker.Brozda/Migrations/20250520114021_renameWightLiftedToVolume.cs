using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseTracker.Brozda.Migrations
{
    /// <inheritdoc />
    public partial class renameWightLiftedToVolume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeightLifted",
                table: "Exercises",
                newName: "Volume");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Volume",
                table: "Exercises",
                newName: "WeightLifted");
        }
    }
}
