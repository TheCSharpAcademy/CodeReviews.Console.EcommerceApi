using ExerciseTracker.KamilKolanowski.Models;

namespace ExerciseTracker.KamilKolanowski.Enums;

internal class ExerciseTrackerMenu
{
    internal enum Menu
    {
        AddExercise,
        RemoveExercise,
        EditExercise,
        ReadExercises,
        Exit,
    }

    internal static Dictionary<Menu, string> ExerciseTrackerMenuDictionary { get; } =
        new()
        {
            { Menu.AddExercise, "Add Exercise" },
            { Menu.RemoveExercise, "Remove Exercise" },
            { Menu.EditExercise, "Edit Exercise" },
            { Menu.ReadExercises, "Read Exercises" },
            { Menu.Exit, "Exit" },
        };

    internal enum ColumnsToEdit
    {
        Name,
        DateStart,
        DateEnd,
        Comment,
    }
}
