using ExerciseTracker.Dejmenek.Models;
using ExerciseTracker.Dejmenek.Services;

namespace ExerciseTracker.Dejmenek.Controllers;
public class ExercisesController
{
    private readonly IExerciseService _exerciseService;

    public ExercisesController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    public void AddExercise()
    {
        _exerciseService.AddExercise();
    }

    public void RemoveExercise()
    {
        _exerciseService.DeleteExercise();
    }

    public void UpadateExercise()
    {
        _exerciseService.UpdateExercise();
    }

    public List<ExerciseReadDTO> GetExercises()
    {
        return _exerciseService.GetExercises();
    }
}
