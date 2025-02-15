using ExerciseTracker.Models;

namespace ExerciseTracker.Services
{
    public interface IService
    {
        void Add(Exercise exercise);
        void Delete(Exercise exercise);
        void Update(Exercise exercise);
        Exercise GetById(int id);
        IEnumerable<Exercise> GetAll();
        IEnumerable<Exercise> GetLast10();

    }
}