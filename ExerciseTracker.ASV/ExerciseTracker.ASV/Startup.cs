using ExerciseTracker.ASV.Controllers;

namespace ExerciseTracker.ASV;

public class Startup : IStartup
{
    private readonly IExerciseController _exerciseController;

    public Startup(IExerciseController exerciseController)
    {
        _exerciseController = exerciseController;
    }
    public async Task Run()
    {
       await _exerciseController.Start();
    }
}