using System.ComponentModel;

namespace ExerciseTracker.StevieTV.UserInterface;

public enum MainMenuOptions
{
    [Description("View Exercises")]
    ViewExercises,
    [Description("Add an Exercise")]
    AddExercise,
    [Description("Delete an Exercise")]
    DeleteExercise,
    [Description("Exit")]
    Exit
}