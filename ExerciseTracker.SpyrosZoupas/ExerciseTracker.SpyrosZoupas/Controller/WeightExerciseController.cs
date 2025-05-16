using ExerciseTracker.SpyrosZoupas.DAL.Model;
using ExerciseTracker.SpyrosZoupas.DAL.Repository;

namespace ExerciseTracker.SpyrosZoupas.Controller;

public class WeightExerciseController
{
    private IRepository<WeightExercise> _repository;

    public WeightExerciseController(IRepository<WeightExercise> repository)
    {
        _repository = repository;
    }

    public void InsertWeightExercise(WeightExercise exercise)
    {
        _repository.Insert(exercise);
    }

    public void DeleteWeightExercise(WeightExercise exercise)
    {
        _repository.Delete(exercise);
    }

    public void UpdateWeightExercise(WeightExercise exercise)
    {
        _repository.Update(exercise);
    }

    public WeightExercise GetWeightExerciseById(int id)
    {
        return _repository.GetById(id);
    }

    public IEnumerable<WeightExercise> GetAllWeightExercises()
    {
        return _repository.GetAll();
    }
}
