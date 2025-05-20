using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseTracker.Brozda.Migrations
{
    /// <inheritdoc />
    public partial class addUnitToExType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "ExerciseTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ExerciseTypes");
        }
    }
}
