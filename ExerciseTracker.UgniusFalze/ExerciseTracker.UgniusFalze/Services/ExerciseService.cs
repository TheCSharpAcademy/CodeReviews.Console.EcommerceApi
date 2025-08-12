using ExerciseTracker.UgniusFalze.Models;
using ExerciseTracker.UgniusFalze.Repositories;

namespace ExerciseTracker.UgniusFalze.Services;

public class ExerciseService(IExerciseRepository exerciseRepository) : IExerciseService
{
    private readonly IExerciseRepository ExerciseRepository = exerciseRepository;
    
    public List<Pullup> GetExercises()
    {
        return ExerciseRepository.GetExercises();
    }

    public bool DeleteExercise(int id)
    {
        return ExerciseRepository.DeleteExercise(id);
    }

    public bool UpdateExercise(Pullup pullup)
    {
        return ExerciseRepository.UpdateExercise(pullup);
    }

    public bool AddExercise(Pullup pullup)
    {
        return ExerciseRepository.InsertExercise(pullup);
    }
    
}