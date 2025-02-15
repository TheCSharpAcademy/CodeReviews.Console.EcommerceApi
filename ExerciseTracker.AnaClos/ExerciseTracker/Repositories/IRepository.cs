using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories;

public interface IRepository
{
    Exercise GetById(int id);
    IEnumerable<Exercise> GetAll();
    void Add(Exercise exercise);
    void Update(Exercise exercise);
    void Delete(Exercise exercise);
}