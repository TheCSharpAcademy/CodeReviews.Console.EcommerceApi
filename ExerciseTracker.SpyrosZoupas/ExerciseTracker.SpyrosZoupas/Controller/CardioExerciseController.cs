using ExerciseTracker.SpyrosZoupas.DAL.Model;
using ExerciseTracker.SpyrosZoupas.DAL.Repository;

namespace ExerciseTracker.SpyrosZoupas.Controller;

public class CardioExerciseController
{
    private IRepositoryDapper<CardioExercise> _repository;

    public CardioExerciseController(IRepositoryDapper<CardioExercise> repository)
    {
        _repository = repository;
    }

    public void InsertCardioExercise(CardioExercise exercise)
    {
        _repository.Insert(exercise);
    }

    public void DeleteCardioExercise(CardioExercise exercise)
    {
        _repository.Delete(exercise);
    }

    public void UpdateCardioExercise(CardioExercise exercise)
    {
        _repository.Update(exercise);
    }

    public CardioExercise GetCardioExerciseById(int id)
    {
        return _repository.GetById(id);
    }

    public IEnumerable<CardioExercise> GetAllCardioExercises()
    {
        return _repository.GetAll();
    }
}
