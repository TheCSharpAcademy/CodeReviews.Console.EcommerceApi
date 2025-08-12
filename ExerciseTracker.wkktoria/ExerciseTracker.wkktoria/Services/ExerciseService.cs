using ExerciseTracker.wkktoria.Data.Models;
using ExerciseTracker.wkktoria.Data.Repositories;

namespace ExerciseTracker.wkktoria.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public List<Exercise> GetAllExercises()
    {
        try
        {
            return _exerciseRepository.GetAllExercises();
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
            return _exerciseRepository.GetExercise(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Exercise AddExercise(Exercise exercise)
    {
        try
        {
            return _exerciseRepository.AddExercise(exercise);
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
            return _exerciseRepository.UpdateExercise(exercise);
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
            _exerciseRepository.DeleteExercise(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}