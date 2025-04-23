using ExerciseTracker.Helper;
using ExerciseTracker.Interfaces;
using ExerciseTracker.Models;

namespace ExerciseTracker.Controllers;

public class ExerciseController
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    public async Task<Result<List<Pushup>>> GetAllExercisesAsync()
    {
        var exercises = await _exerciseService.GetAll();

        if (exercises == null || exercises.Count == 0)
            return Result<List<Pushup>>.FailResult("No exercises found.");

        return Result<List<Pushup>>.SuccessResult(exercises, "Exercises retrieved successfully.");
    }

    public async Task<Result<Pushup>> GetExerciseByIdAsync(int id)
    {
        var exercise = await _exerciseService.GetById(id);
        if (exercise == null)
            return Result<Pushup>.FailResult("Exercise not found.");
        return Result<Pushup>.SuccessResult(exercise);
    }

    public async Task<Result<Pushup>> CreateExerciseAsync(Pushup newPushup)
    {
        if (newPushup == null)
        {
            return Result<Pushup>.FailResult("Invalid exercise data.");
        }
        await _exerciseService.Create(newPushup);
        return Result<Pushup>.SuccessResult(newPushup, "Exercise created successfully.");
    }

    public async Task<Result<Pushup>> UpdateExerciseAsync(int id, Pushup updatedExercise)
    {
        if (id != updatedExercise.Id)
        {
            return Result<Pushup>.FailResult("Mismatched exercise ID.");
        }
        var existingExercise = await _exerciseService.GetById(id);
        if (existingExercise == null)
            return Result<Pushup>.FailResult("Exercise not found.");

        await _exerciseService.Update(updatedExercise);
        return Result<Pushup>.SuccessResult(updatedExercise, "Exercise updated successfully.");
    }

    public async Task<Result<Pushup>> DeleteExerciseAsync(int id)
    {
        var exercise = await _exerciseService.GetById(id);
        if (exercise == null)
        {
            return Result<Pushup>.FailResult("Exercise not found.");
        }
        await _exerciseService.Delete(exercise);
        return Result<Pushup>.SuccessResult(exercise, "Exercise deleted successfully.");
    }
}