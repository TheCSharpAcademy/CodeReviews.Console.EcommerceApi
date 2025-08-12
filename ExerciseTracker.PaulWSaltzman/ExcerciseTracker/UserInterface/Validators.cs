using ExerciseTracker.Models;

namespace ExerciseTracker.UserInterface;

internal class Validators
{
    internal static Exercise CalculateSessionLength(Exercise exercise)
    {
        TimeSpan maxDuration = TimeSpan.FromHours(23);
        TimeSpan calculateDuration = (TimeSpan)(exercise.DateEnd - exercise.DateStart);
        exercise.Duration = calculateDuration <= maxDuration ? calculateDuration : maxDuration;

        return exercise;
    }

    internal static bool DateValidator(Exercise exercise)
    {
        if (exercise.DateStart > exercise.DateEnd)
        {
            Console.WriteLine("The session start time can not be after the end time.");
            Console.ReadKey();
            return false;
        }
        else
        {
            return true;
        }
    }
}
