using ExerciseTracker.KamilKolanowski.Models;

namespace ExerciseTracker.KamilKolanowski.Interfaces;

public interface IExerciseRepository
{
    IEnumerable<Exercise?> GetExercises(string type);
    Exercise? GetExercise(int id);
    void Insert(Exercise exercise);
    void Update(Exercise exercise);
    void Delete(int id);
    void Save();
}
