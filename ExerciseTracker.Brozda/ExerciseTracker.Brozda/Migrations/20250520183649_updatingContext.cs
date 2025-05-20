using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseTracker.Brozda.Migrations
{
    /// <inheritdoc />
    public partial class updatingContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesWeight_ExerciseTypes_TypeId",
                table: "ExercisesWeight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExercisesWeight",
                table: "ExercisesWeight");

            migrationBuilder.RenameTable(
                name: "ExercisesWeight",
                newName: "Exercise");

            migrationBuilder.RenameIndex(
                name: "IX_ExercisesWeight_TypeId",
                table: "Exercise",
                newName: "IX_Exercise_TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_ExerciseTypes_TypeId",
                table: "Exercise",
                column: "TypeId",
                principalTable: "ExerciseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_ExerciseTypes_TypeId",
                table: "Exercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise");

            migrationBuilder.RenameTable(
                name: "Exercise",
                newName: "ExercisesWeight");

            migrationBuilder.RenameIndex(
                name: "IX_Exercise_TypeId",
                table: "ExercisesWeight",
                newName: "IX_ExercisesWeight_TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExercisesWeight",
                table: "ExercisesWeight",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesWeight_ExerciseTypes_TypeId",
                table: "ExercisesWeight",
                column: "TypeId",
                principalTable: "ExerciseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
