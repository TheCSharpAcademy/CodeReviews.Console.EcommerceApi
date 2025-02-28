
namespace ExerciseTracker.cacheMe512;

internal class ExerciseController
{
    private readonly ExerciseService _exerciseService;

    public ExerciseController(ExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    public IEnumerable<Exercise> GetExercises() => _exerciseService.GetAllExercises();

    public Exercise GetExercise(int id) => _exerciseService.GetExerciseById(id);

    public void CreateExercise(Exercise exercise) => _exerciseService.AddExercise(exercise);

    public void UpdateExercise(Exercise exercise) => _exerciseService.UpdateExercise(exercise);

    public void DeleteExercise(Exercise exercise) => _exerciseService.DeleteExercise(exercise);
}
