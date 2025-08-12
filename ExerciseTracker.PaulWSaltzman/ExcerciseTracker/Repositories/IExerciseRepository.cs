using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories;

public interface IExerciseRepository
{
    public IEnumerable<Exercise> GetAll();
    public Exercise GetById(int id);
    public Exercise Insert(Exercise exercise);
    public Exercise Update(Exercise exercise);
    public void Delete(Exercise exercise);
    public void Save();

}
