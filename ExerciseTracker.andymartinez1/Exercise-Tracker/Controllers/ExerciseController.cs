using Exercise_Tracker.Models;
using Exercise_Tracker.Services;
using Exercise_Tracker.Utils;
using Exercise_Tracker.Views;

namespace Exercise_Tracker.Controllers;

public class ExerciseController
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    public void GetAllExercises()
    {
        var exercises = _exerciseService.GetAllExercises();

        UserInterface.ShowAllExercises(exercises);
    }

    public void GetExerciseById()
    {
        var exercises = _exerciseService.GetAllExercises();

        var exerciseId = Helpers.GetExerciseId(exercises);

        var selectedShift = _exerciseService.GetExerciseById(exerciseId);

        if (exercises.Count > 0)
            UserInterface.ShowExerciseDetails(selectedShift);
        else
            return;
    }

    public void AddExercise()
    {
        _exerciseService.AddExercise();
    }

    public void UpdateExercise()
    {
        var exercises = _exerciseService.GetAllExercises();

        var exerciseId = Helpers.GetExerciseId(exercises);

        var shiftToUpdate = _exerciseService.GetExerciseById(exerciseId);

        if (exercises.Count > 0)
            _exerciseService.UpdateExercise(shiftToUpdate);
        else
            return;
    }

    public void DeleteExercise()
    {
        var exercises = _exerciseService.GetAllExercises();

        var exerciseId = Helpers.GetExerciseId(exercises);

        if (exercises.Count > 0)
            _exerciseService.DeleteExercise(exerciseId);
        else
            return;
    }
}
