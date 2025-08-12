using ExerciseTracker.hasona23.Models;
using ExerciseTracker.hasona23.Repository;

namespace ExerciseTracker.hasona23.Services;

public class ExerciseService(IExerciseRepository exerciseRepository) : IExerciseService
{
    public List<Exercise> GetAllExercises()
    {
        return exerciseRepository.GetAllExercises();
    }

    public Exercise? GetExercise(int id)
    {
        return exerciseRepository.GetExercise(id);
    }

    public void AddExercise(ExerciseCreate exercise)
    {
        exerciseRepository.AddExercise(exercise);
    }

    public bool UpdateExercise(ExerciseUpdate newExercise)
    {
        return exerciseRepository.UpdateExercise(newExercise);
    }

    public bool DeleteExercise(int id)
    {
        return exerciseRepository.DeleteExercise(id);
    }
}