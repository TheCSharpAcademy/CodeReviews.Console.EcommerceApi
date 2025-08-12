using edvaudin.ExerciseTracker.Models;
using edvaudin.ExerciseTracker.Repositories;
using edvaudin.ExerciseTracker.Visulisation;

namespace edvaudin.ExerciseTracker.Services;

internal class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository exerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        this.exerciseRepository = exerciseRepository;
    }

    public void ViewExercises()
    {
        List<Exercise> exercises = exerciseRepository.GetExercises().ToList();
        Viewer.DisplayExerciseTable(exercises);
    }

    public void AddExercise(DateTime start, DateTime end, string? comments)
    {
        Exercise exercise = new()
        {
            DateStart = start,
            DateEnd = end,
            Duration = end - start,
            Comments = comments
        };
        exerciseRepository.AddExercise(exercise);
        Console.WriteLine("New exercise added.");
    }

    public void DeleteExercise(int id)
    {
        if (!exerciseRepository.TryGetExerciseById(id, out Exercise? exerciseToDelete))
        {
            Console.WriteLine("We could not find the exercise you were looking to delete.");
            return;
        }
        if (exerciseToDelete == null)
        {
            Console.WriteLine("Something went wrong retrieving this exercise.");
            return;
        }
        exerciseRepository.DeleteExercise(exerciseToDelete);
        Console.WriteLine("Successfully deleted exercise.");
    }

    public void UpdateExercise(int id, DateTime start, DateTime end, string comments)
    {
        if (!exerciseRepository.TryGetExerciseById(id, out Exercise? exerciseToUpdate))
        {
            Console.WriteLine("We could not find the exercise you were looking to update.");
            return;
        }
        if (exerciseToUpdate == null)
        {
            Console.WriteLine("Something went wrong retrieving this exercise.");
            return;
        }
        Exercise updatedExercise = new()
        {
            DateStart = start,
            DateEnd = end,
            Duration = end - start,
            Comments = comments
        };
        exerciseRepository.UpdateExercise(exerciseToUpdate, updatedExercise);
        Console.WriteLine("Successfully updated exercise.");
    }
}
