using ExerciseTracker.StevieTV.Models;
using ExerciseTracker.StevieTV.Services;

namespace ExerciseTracker.StevieTV.Controllers;

public class ExerciseController
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    public List<Exercise> GetExercises()
    {
        return _exerciseService.GetExercises();
    }

    public bool AddExercise(Exercise exercise)
    {
        return _exerciseService.AddExercise(exercise);
    }

    public bool RemoveExercise(Exercise exercise)
    {
        return _exerciseService.RemoveExercise(exercise);
    }
}