using ExerciseTrackerAPI.Models;

namespace ExerciseTrackerAPI.Repositories.ExerciseRepositories;

public interface IExerciseRepository
{
    IEnumerable<Exercise> GetAll();

    Exercise? GetById(int id);

    void Create(Exercise exercise);

    void Update(int id, Exercise exercise);

    void Delete(int id);
}
