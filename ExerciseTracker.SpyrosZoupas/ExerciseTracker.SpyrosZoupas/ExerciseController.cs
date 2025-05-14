using ExerciseTracker.SpyrosZoupas.DAL;
using ExerciseTracker.SpyrosZoupas.DAL.Repository;

namespace ExerciseTracker.SpyrosZoupas;

public class ExerciseController
{
    private IRepository<Exercise> _repository;

    public ExerciseController(IRepository<Exercise> repository)
    {
        _repository = repository;
    }

    public void InsertExercise(Exercise exercise)
    {
        _repository.Insert(exercise);
    }

    public void DeleteExercise(Exercise exercise)
    {
        _repository.Delete(exercise);
    }

    public void UpdateExercise(Exercise exercise)
    {
        _repository.Update(exercise);
    }

    public Exercise GetExerciseById(int id)
    {
        return _repository.GetById(id);
    }

    public IEnumerable<Exercise> GetAllExercises()
    {
        return _repository.GetAll();
    }
}
