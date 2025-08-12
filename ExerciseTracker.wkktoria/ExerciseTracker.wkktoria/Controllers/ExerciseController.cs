using ExerciseTracker.wkktoria.Data.Models;
using ExerciseTracker.wkktoria.Services;

namespace ExerciseTracker.wkktoria.Controllers;

public class ExerciseController : IExerciseController
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    public List<Exercise> GetAllExercises()
    {
        try
        {
            return _exerciseService.GetAllExercises();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Exercise? GetExercise(int id)
    {
        try
        {
            return _exerciseService.GetExercise(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Exercise? AddExercise(Exercise exercise)
    {
        try
        {
            return _exerciseService.AddExercise(exercise);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Exercise UpdateExercise(Exercise exercise)
    {
        try
        {
            return _exerciseService.UpdateExercise(exercise);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void DeleteExercise(int id)
    {
        try
        {
            _exerciseService.DeleteExercise(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}