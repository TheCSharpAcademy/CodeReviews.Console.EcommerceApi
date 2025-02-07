using ExerciseTrackerAPI.Models;

namespace ExerciseTrackerAPI.Repositories.ExerciseTypeRepositories;

public interface IExerciseTypeRepository
{
    IEnumerable<ExerciseType> GetAll();

    ExerciseType? GetById(int id);

    void Create(ExerciseType exerciseType);

    void Update(int id, ExerciseType exerciseType);

    void Delete(int id);
}
