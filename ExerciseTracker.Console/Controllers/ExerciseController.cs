using ExerciseTracker.Niasua.Services;
using ExerciseTracker.Niasua.UserInput;

namespace ExerciseTracker.Niasua.Controllers;

public class ExerciseController
{
    private readonly ExerciseService _service;

    public ExerciseController(ExerciseService service)
    {
        _service = service;
    }
    
    public async Task AddExercise()
    {
        var exercise = UserInputHandler.GetExerciseInput();

        var success = await _service.CreateExerciseAsync(exercise);

        if (success)
            Console.WriteLine("Exercise successfully added.");
        else
            Console.WriteLine("Failed to add exercise.");
    }
}
