using ExerciseTracker.Dejmenek.Data.Interfaces;
using ExerciseTracker.Dejmenek.Models;

namespace ExerciseTracker.Dejmenek.Data.Repositories;
public class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseContext _context;

    public ExerciseRepository(ExerciseContext context)
    {
        _context = context;
    }

    public void AddExercise(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        _context.SaveChanges();
    }

    public void DeleteExercise(int exerciseId)
    {
        var exercise = GetExerciseById(exerciseId);
        _context.Exercises.Remove(exercise);
        _context.SaveChanges();
    }

    public Exercise GetExerciseById(int exerciseId)
    {
        return _context.Exercises.Find(exerciseId)!;
    }

    public List<Exercise> GetExercises()
    {
        return _context.Exercises.ToList();
    }

    public void UpdateExercise(int exerciseId, ExerciseUpdateDTO exerciseDto)
    {
        Exercise oldExercise = GetExerciseById(exerciseId);

        oldExercise.EndTime = exerciseDto.EndTime;
        oldExercise.Duration = exerciseDto.Duration;
        oldExercise.Comments = exerciseDto.Comments;

        _context.SaveChanges();
    }
}
