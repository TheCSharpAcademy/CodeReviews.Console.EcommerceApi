namespace ExerciseTracker.JsPeanut
{
    public interface IExerciseRepository
    {
        void Insert(Exercise exercise);
        void Delete(int id);
        void Update(Exercise exercise);
        void Save();
        Exercise Get(int id);
        IEnumerable<Exercise> GetAll();
    }
}
