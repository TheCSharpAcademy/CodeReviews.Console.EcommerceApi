using Kmakai.ExerciseTracker.Models;
using Kmakai.ExerciseTracker.Repositories;

namespace Kmakai.ExerciseTracker.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository ExerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        ExerciseRepository = exerciseRepository;
    }

    public async Task<Exercise> GetAsync(int id)
    {
        return await ExerciseRepository.GetAsync(id);
    }

    public async Task<IEnumerable<Exercise>> GetAllAsync()
    {
        return await ExerciseRepository.GetAllAsync();
    }

    public async Task<Exercise> AddAsync(Exercise entity)
    {
        return await ExerciseRepository.AddAsync(entity);
    }

    public async Task<Exercise> UpdateAsync(Exercise entity)
    {
        return await ExerciseRepository.UpdateAsync(entity);
       
    }

    public async Task<Exercise> DeleteAsync(int id)
    {
        return await ExerciseRepository.DeleteAsync(id);
    }

}
