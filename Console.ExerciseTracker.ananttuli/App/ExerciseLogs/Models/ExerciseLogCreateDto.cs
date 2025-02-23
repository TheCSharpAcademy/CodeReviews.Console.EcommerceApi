namespace App.ExerciseLogs.Models;

public record class ExerciseLogCreateDto(
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Comments
);