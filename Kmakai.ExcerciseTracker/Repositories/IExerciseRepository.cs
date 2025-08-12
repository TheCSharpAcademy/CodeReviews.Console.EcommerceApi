using Kmakai.ExerciseTracker.Models;

namespace Kmakai.ExerciseTracker.Repositories;

public interface IExerciseRepository: IRepository<Exercise>
{
    Exercise Get(int id);

    IEnumerable<Exercise> GetAll();

    Exercise Add(Exercise entity);

    Exercise Update(Exercise entity);

    Exercise Delete(int id);
}
