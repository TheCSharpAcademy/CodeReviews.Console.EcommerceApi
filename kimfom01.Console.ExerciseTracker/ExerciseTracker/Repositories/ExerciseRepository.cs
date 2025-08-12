using ExerciseTracker.Data;
using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories;

public class ExerciseRepository : Repository, IExerciseRepository
{
    public ExerciseRepository(ExerciseContext exerciseContext) : base(exerciseContext)
    {
    }

    public override bool UpdateExercise(int id, Exercise exercise)
    {
        var oldExercise = GetExerciseById(id);

        if(oldExercise is null)
        {
            Console.Clear();
            Console.WriteLine("This exercise does not exist!");
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
            return false;
        }

        var duration = exercise.EndDate - oldExercise.StartDate;

        exercise.Duration = duration;

        return base.UpdateExercise(id, exercise);
    }
}