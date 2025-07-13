using ExerciseTrackerApi.Models;
using ExerciseTrackerApi.Interfaces;
public class ExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
    {
        return await _exerciseRepository.GetAllExercisesAsync();
    }

    public async Task<Exercise> GetExerciseByIdAsync(int id)
    {
        return await _exerciseRepository.GetExerciseByIdAsync(id);
    }

    public async Task AddExerciseAsync(Exercise exercise)
    {
        await _exerciseRepository.AddExerciseAsync(exercise);
    }

    public async Task UpdateExerciseAsync(Exercise exercise)
    {
        await _exerciseRepository.UpdateExerciseAsync(exercise);
    }

    public async Task DeleteExerciseAsync(int id)
    {
        await _exerciseRepository.DeleteExerciseAsync(id);
    }
}