using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseTracker.KamilKolanowski.Migrations
{
    /// <inheritdoc />
    public partial class extendSchemaFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExerciseType",
                schema: "TCSA",
                table: "Exercises",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExerciseType",
                schema: "TCSA",
                table: "Exercises",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
