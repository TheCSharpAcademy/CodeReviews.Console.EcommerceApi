using ExerciceTracker.Data.Models;

namespace ExerciceTracker.Data.Repositories
{
    internal interface IExerciseRepository
    {
        public IEnumerable<Exercise> GetAll();
        public void Add(Exercise exercise);
        public void Delete(int id);
    }
}