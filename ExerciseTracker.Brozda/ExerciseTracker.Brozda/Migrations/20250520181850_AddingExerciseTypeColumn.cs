using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseTracker.Brozda.Migrations
{
    /// <inheritdoc />
    public partial class AddingExerciseTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "ExercisesWeight",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExercisesWeight_TypeId",
                table: "ExercisesWeight",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesWeight_ExerciseTypes_TypeId",
                table: "ExercisesWeight",
                column: "TypeId",
                principalTable: "ExerciseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesWeight_ExerciseTypes_TypeId",
                table: "ExercisesWeight");

            migrationBuilder.DropIndex(
                name: "IX_ExercisesWeight_TypeId",
                table: "ExercisesWeight");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "ExercisesWeight");
        }
    }
}
