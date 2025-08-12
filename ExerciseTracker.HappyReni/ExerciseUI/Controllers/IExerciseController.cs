namespace ExerciseUI.Controllers
{
    internal interface IExerciseController<T> where T : class
    {
        IEnumerable<T> GetExercises();
        bool AddExercise(T exercise);
        bool RemoveExercise(int id);
        bool UpdateExercise(T exercise);
        T GetExercise(int id);
    }
}
