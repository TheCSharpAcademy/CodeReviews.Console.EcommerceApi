using ExerciseTracker.Models;
using ExerciseTracker.Repository;
using ExerciseTracker.Services;

namespace ExerciseTracker.Controllers;

public class ExerciseController
{
    private readonly IExerciseService  _service;
    
    public ExerciseController(IExerciseService service)
    {
       _service = service;
    }

    public async Task AddExercise(Exercise exercise)
    {
        await _service.AddExercise(exercise);
    }

    public async Task UpdateExercise(Exercise exercise)
    {
        await _service.EditExercise(exercise);  
    }

    public async Task DeleteExercise(Exercise exercise)
    {
        await _service.DeleteExercise(exercise);
    }

    public async Task<IEnumerable<Exercise>> GetAll()
    {
        return await _service.GetAllExercises();
    }
}