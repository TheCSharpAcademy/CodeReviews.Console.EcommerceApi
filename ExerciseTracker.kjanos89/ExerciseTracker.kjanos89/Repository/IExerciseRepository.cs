using ExerciseTracker.kjanos89.Models;

namespace ExerciseTracker.kjanos89.Repository;

public interface IExerciseRepository
{
    IEnumerable<Exercise> ListAll();
    void Create(Exercise exercise);
    Exercise Read(int id);
    void Update(Exercise exercise);
    void Delete(int id);
    bool Exists(int id);
}