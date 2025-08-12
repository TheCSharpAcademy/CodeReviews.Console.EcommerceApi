using ExerciseTracker.Forser.Models;

namespace ExerciseTracker.Forser.Repository
{
    internal interface IExerciseReposoitory
    {
        public IEnumerable<Exercise> GetExercises();
        public bool GetExerciseById(int id, out Exercise? exercise);
        public Exercise EditExerciseById(int id);
        public bool AddExercise(Exercise exercise);
        public bool EditExercise(Exercise exercise);
        public void DeleteExercise(Exercise exercise);
    }
}