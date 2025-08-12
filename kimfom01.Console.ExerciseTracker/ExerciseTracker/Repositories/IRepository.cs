using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories;

public interface IRepository
{
    public IEnumerable<Exercise> GetExercises();
    public Exercise? GetExerciseById(int id);
    public void AddExercise(Exercise exercise);
    public bool UpdateExercise(int id, Exercise exercise);
    public bool DeleteExercise(int id);
    public int SaveChanges();
}