using ExerciseTracker.Niasua.Models;
using ExerciseTracker.Niasua.Repositories;
using ExerciseTracker.Niasua.Services;
using Spectre.Console;
using System.Threading.Tasks;

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

    public static async Task<bool> ExerciseExistsById(int id, ExerciseService service)
    {
        return await service.GetExerciseByIdAsync(id) != null;
    }
}
