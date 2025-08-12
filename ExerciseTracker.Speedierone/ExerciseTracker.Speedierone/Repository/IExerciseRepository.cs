using ExerciseTracker.Speedierone.Model;

namespace ExerciseTracker.Speedierone.Repository
{
    public interface IExerciseRepository
    {
        IEnumerable<Exercises> GetAll();
        List<Exercises> GetById(int id);
        void Add(Exercises exercises);
        void Update(Exercises exercises);
        void Delete(int id);
        void SaveChanges();
    }
}
