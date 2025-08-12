using ExerciseTracker.UgniusFalze.Models;
using ExerciseTracker.UgniusFalze.Services;

namespace ExerciseTracker.UgniusFalze.Controllers;

public class ExerciseController
{
    private readonly IExerciseService ExerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        ExerciseService = exerciseService;
    }

    public List<Pullup> GetExercises()
    {
        return ExerciseService.GetExercises();
    }

    public bool AddExercise(Pullup pullup)
    {
        return ExerciseService.AddExercise(pullup);
    }

    public bool UpdateExercise(Pullup pullup)
    {
        return ExerciseService.UpdateExercise(pullup);
    }

    public bool DeleteExercise(int id)
    {
        return ExerciseService.DeleteExercise(id);
    }
}