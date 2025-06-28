using ExerciseTracker.Models;
using ExerciseTracker.Repository;

namespace ExerciseTracker.Services;

public class ExerciseService : IExerciseService
{
    private readonly IRepository<Exercise> _repository;

    public ExerciseService(IRepository<Exercise> repository)
    {
        _repository = repository;
    }

    public async Task AddExercise(Exercise exercise)
    {
        await _repository.Insert(exercise);
    }

    public async Task DeleteExercise(Exercise exercise)
    {
        await _repository.Delete(exercise);
    }

    public async Task EditExercise(Exercise exercise)
    {
        await _repository.Update(exercise);
    }

    public async Task<IEnumerable<Exercise>> GetAllExercises()
    {
        return await _repository.GetAll();
    }
}