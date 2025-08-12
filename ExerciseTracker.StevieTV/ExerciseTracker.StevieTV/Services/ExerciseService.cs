using ExerciseTracker.StevieTV.Models;
using ExerciseTracker.StevieTV.Repositories;

namespace ExerciseTracker.StevieTV.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public List<Exercise> GetExercises()
    {
        return _exerciseRepository.GetExercises();
    }

    public bool AddExercise(Exercise exercise)
    {
        return _exerciseRepository.AddExercise(exercise);
    }

    public bool RemoveExercise(Exercise exercise)
    {
        return _exerciseRepository.RemoveExercise(exercise);
    }
}