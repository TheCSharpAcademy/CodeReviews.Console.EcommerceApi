using ExerciseTracker.UgniusFalze.Models;

namespace ExerciseTracker.UgniusFalze.Services;

public interface IExerciseService
{
    public List<Pullup> GetExercises();
    public bool UpdateExercise(Pullup pullup);
    public bool AddExercise(Pullup pullup);
    public bool DeleteExercise(int id);
}