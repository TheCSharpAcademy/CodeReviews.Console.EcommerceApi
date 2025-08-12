using edvaudin.ExerciseTracker.Context;
using edvaudin.ExerciseTracker.Models;

namespace edvaudin.ExerciseTracker.Repositories;

internal class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseContext exerciseContext;
    public ExerciseRepository(ExerciseContext exerciseContext)
    {
        this.exerciseContext = exerciseContext;
    }

    public void AddExercise(Exercise exercise)
    {
        exerciseContext.Add(exercise);
        exerciseContext.SaveChanges();
    }

    public void DeleteExercise(Exercise exercise)
    {
        exerciseContext.Remove(exercise);
        exerciseContext.SaveChanges();
    }

    public bool TryGetExerciseById(int id, out Exercise? exercise)
    {
        exercise = null;
        Exercise? _ = exerciseContext.Exercises.Find(id);
        if (_ == null)
        {
            return false;
        }
        else
        {
            exercise = _;
            return true;
        }
    }

    public IEnumerable<Exercise> GetExercises()
    {
        return exerciseContext.Exercises;
    }

    public void UpdateExercise(Exercise exerciseToUpdate, Exercise updatedExercise)
    {
        exerciseToUpdate.DateStart = updatedExercise.DateStart;
        exerciseToUpdate.DateEnd = updatedExercise.DateEnd;
        exerciseToUpdate.Duration = updatedExercise.Duration;
        exerciseToUpdate.Comments = updatedExercise.Comments;
        exerciseContext.Exercises.Entry(exerciseToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        exerciseContext.SaveChanges();
    }
}
