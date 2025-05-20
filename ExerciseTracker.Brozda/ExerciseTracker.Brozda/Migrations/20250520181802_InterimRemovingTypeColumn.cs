using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseTracker.Brozda.Migrations
{
    /// <inheritdoc />
    public partial class InterimRemovingTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ExercisesWeight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ExercisesWeight",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
