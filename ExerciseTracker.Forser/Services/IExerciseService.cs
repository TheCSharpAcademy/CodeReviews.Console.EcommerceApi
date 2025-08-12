namespace ExerciseTracker.Forser.Services
{
    internal interface IExerciseService
    {
        bool AddExercise(DateTime start, DateTime end, string? comments);
        void DeleteExercise(int id);
        Exercise EditExercise(int id);
        bool UpdateExercise(Exercise exercise);
        void DisplayExercises();
    }
}