using ExerciseTracker.Models;

namespace ExerciseTracker.Services
{
    public interface IExerciseService
    {
        Exercise AddExercise(Exercise exercise);
        void DeleteExercise(Exercise exercise);
        List<Exercise> GetAllExercises();
        Exercise UpdateExercise(Exercise exercise);
    }
}