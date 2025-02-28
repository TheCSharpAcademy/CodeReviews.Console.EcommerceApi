
namespace ExerciseTracker.cacheMe512;

internal class ExerciseService
{
    private readonly IExerciseRepository _repository;

    public ExerciseService(IExerciseRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Exercise> GetAllExercises() => _repository.GetAll();

    public Exercise GetExerciseById(int id) => _repository.GetById(id);

    public void AddExercise(Exercise exercise) => _repository.Add(exercise);

    public void UpdateExercise(Exercise exercise) => _repository.Update(exercise);

    public void DeleteExercise(Exercise exercise) => _repository.Delete(exercise);

}
