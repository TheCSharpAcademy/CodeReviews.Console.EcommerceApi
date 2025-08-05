using ExerciseTracker.Niasua.Models;

namespace ExerciseTracker.Niasua.Validators;

public static class ExerciseValidator
{
    public static bool IsValid(Exercise exercise)
    {
        if (exercise.DateEnd <= exercise.DateStart)
        {
            Console.WriteLine("End time must be after start time.");
            return false;
        }

        if (exercise.Duration.TotalSeconds <= 0)
        {
            Console.WriteLine("Duration must be greater than zero");
            return false;
        }

        return true;
    }
}
