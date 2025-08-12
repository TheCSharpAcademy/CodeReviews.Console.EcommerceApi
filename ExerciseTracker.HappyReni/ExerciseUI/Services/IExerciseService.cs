namespace ExerciseUI.Services
{
    public interface IExerciseService<T> where T : class
    {
        public IEnumerable<T> GetExercises();
        public bool AddExercise(T entity);
        public bool RemoveExercise(int id);
        public bool UpdatingExercise(T entity);
        public T GetExercise(int id);
    }
}
