using ExerciseTracker.Speedierone.Model;

namespace ExerciseTracker.Speedierone.Service
{
    public interface IExerciseService
    {
        IEnumerable<Exercises> GetAllExercises();
        List<Exercises> GetExerciseById(int id);
        void AddExercise(Exercises exercise);
        void UpdateExercise(Exercises exercise);
        void DeleteExercise(int id);
    }
}
