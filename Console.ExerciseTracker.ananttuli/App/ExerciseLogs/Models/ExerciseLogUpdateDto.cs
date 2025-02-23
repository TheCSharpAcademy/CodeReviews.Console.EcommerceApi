namespace App.ExerciseLogs.Models;

public record class ExerciseLogUpdateDto(
    int Id,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Comments
);