namespace ExerciseTracker;
public class ExerciseController
{
    private readonly IExerciseRepository _exerciseRepository;
    public ExerciseController(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }
    
    public Exercise GetExerciseById(int id)
    {
        return _exerciseRepository.GetExerciseById(id);
    }
    public IEnumerable<Exercise> GetExercises()
    {
        return _exerciseRepository.GetExercises();
    }
    public void AddExercise(Exercise exercise)
    {
        _exerciseRepository.AddExercise(exercise);
        _exerciseRepository.Save();
    }
    public void DeleteExercise(int id)
    {
        _exerciseRepository.DeleteExercise(id);
        _exerciseRepository.Save();
    }
    public void UpdateExercise(Exercise exercise)
    {
        _exerciseRepository.UpdateExercise(exercise);
        _exerciseRepository.Save();
    }
}