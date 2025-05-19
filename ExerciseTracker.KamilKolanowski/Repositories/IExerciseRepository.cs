using ExerciseTracker.KamilKolanowski.Models;

namespace ExerciseTracker.KamilKolanowski.Repositories;

public interface IExerciseRepository
{
    IEnumerable<Exercise?> GetExercises();
    Exercise? GetExercise(int id);
    void Insert(Exercise exercise);
    void Update(Exercise exercise);
    void Delete(int id);
    void Save();
}
