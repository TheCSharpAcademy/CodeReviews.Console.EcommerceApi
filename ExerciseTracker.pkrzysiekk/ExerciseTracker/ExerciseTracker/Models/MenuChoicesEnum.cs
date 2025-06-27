using System.ComponentModel;

namespace ExerciseTracker.Models;

public enum MenuChoicesEnum
{
    [Description("Add an Exercise")]
    AddExercise,

    [Description("Edit an Exercise")]
    EditExercise,

    [Description("Delete an Exercise")]
    DeleteExercise,

    [Description("List all Exercises")]
    GetAllExercises,

    [Description("Exit")]
    Exit
}