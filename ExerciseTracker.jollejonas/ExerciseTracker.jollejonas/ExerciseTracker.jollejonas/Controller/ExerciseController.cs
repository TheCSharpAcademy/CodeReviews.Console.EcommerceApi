using ExerciseTracker.jollejonas.Models;

namespace ExerciseTracker.jollejonas.Controllers;
public class ExerciseController
{

    private readonly ExerciseService _exerciseService;
    public ExerciseController(ExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    public void AddExercise()
    {
        _exerciseService.AddExercise();
    }

    public void DeleteExercise()
    {
        _exerciseService.DeleteExercise();
    }

    public List<Exercise> GetAllExercises()
    {
        return _exerciseService.GetAllExercises();
    }

    public void UpdateExercise()
    {
        _exerciseService.UpdateExercise();
    }
}


