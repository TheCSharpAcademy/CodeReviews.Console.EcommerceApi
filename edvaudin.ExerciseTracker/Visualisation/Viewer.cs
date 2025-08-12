using edvaudin.ExerciseTracker.Models;

namespace edvaudin.ExerciseTracker.Visulisation;

internal static class Viewer
{
    internal static void DisplayExerciseTable(List<Exercise> exercises)
    {
        foreach (Exercise exercise in exercises)
        {
            Console.WriteLine($"[#{exercise.Id}] Duration: {exercise.Duration} ({exercise.DateStart} - {exercise.DateEnd}) [Comments: {exercise.Comments}]\n");
        }
    }

    internal static void DisplayOptionsMenu()
    {
        Console.WriteLine("+----- Exercise Tracker -----+");
        Console.WriteLine("\nChoose an action from the following list:");
        Console.WriteLine("\tv - View exercises");
        Console.WriteLine("\ta - Add new exercise");
        Console.WriteLine("\td - Delete an exercise");
        Console.WriteLine("\tu - Update an exercise");
        Console.WriteLine("\t0 - Quit this application");
        Console.Write("Your option? ");
    }
}