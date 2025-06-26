using ExerciseTracker.Models;
using ExerciseTracker.Repository;

namespace ExerciseTracker.Controllers;

public class ExerciseController
{
    private readonly IRepository<Exercise> _repository;
    
    public ExerciseController(IRepository<Exercise> repository)
    {
       _repository = repository; 
    }

    public async Task AddExercise(Exercise exercise)
    {
        await _repository.Insert(exercise);
    }

    public async Task UpdateExercise(Exercise exercise)
    {
        await _repository.Update(exercise);
    }

    public async Task DeleteExercise(Exercise exercise)
    {
        await _repository.Delete(exercise);
    }

    public async Task<IEnumerable<Exercise>> GetAll()
    {
        return await _repository.GetAll();
    }
}