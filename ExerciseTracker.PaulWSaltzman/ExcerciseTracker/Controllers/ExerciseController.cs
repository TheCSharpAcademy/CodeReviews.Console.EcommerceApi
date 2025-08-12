using ExerciseTracker.Models;
using ExerciseTracker.Repositories;
using ExerciseTracker.Services;

namespace ExerciseTracker.Controllers;

public class ExerciseController
{

    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService excerciseService)
    {
        _exerciseService = excerciseService;
    }

    public Exercise AddExercise(Exercise exercise)
    {
        return _exerciseService.AddExercise(exercise);
    }

    public void DeleteExercise(Exercise exercise)
    {
        _exerciseService.DeleteExercise(exercise);

    }

    public List<Exercise> GetAllExercises()
    {
        return _exerciseService.GetAllExercises();
    }

    public Exercise UpdateExercise(Exercise exercise)
    {
        return _exerciseService.UpdateExercise(exercise);
    }
}
