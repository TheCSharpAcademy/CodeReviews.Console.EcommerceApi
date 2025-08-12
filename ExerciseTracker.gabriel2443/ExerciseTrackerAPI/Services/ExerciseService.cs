using ExerciseTracker.Models;
using ExerciseTrackerAPI.Repository;

namespace ExerciseTracker.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<List<Exercise>> GetAllExercises()
    {
        return await _exerciseRepository.GetAllExercises();
    }

    public async Task<Exercise> GetExerciseById(int id)
    {
        return await _exerciseRepository.GetExerciseById(id);
    }

    public async Task CreateExercise(Exercise exercise)
    {
        await _exerciseRepository.CreateExercise(exercise);
    }

    public async Task UpdateExercise(int id, Exercise exerciseToUpdate)
    {
        await _exerciseRepository.UpdateExercise(id, exerciseToUpdate);
    }

    public async Task DeleteExercise(int id)
    {
        await _exerciseRepository.DeleteExercise(id);
    }
}