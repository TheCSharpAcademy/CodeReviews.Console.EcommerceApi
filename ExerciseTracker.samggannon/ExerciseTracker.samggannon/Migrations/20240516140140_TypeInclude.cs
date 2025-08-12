using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseTracker.samggannon.Migrations
{
    /// <inheritdoc />
    public partial class TypeInclude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ExerciseSet",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ExerciseSet");
        }
    }
}
