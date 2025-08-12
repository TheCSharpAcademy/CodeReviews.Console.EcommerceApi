using ExerciceTracker.Data.Models;

namespace ExerciceTracker.Data.Repositories
{
    internal class ExerciseRepository : IExerciseRepository
    {
        private readonly ExerciseDbContext _context;
        public ExerciseRepository(ExerciseDbContext context)
        {
            _context = context;
        }

        public void Add(Exercise exercise)
        {
            _context.Add(exercise);
            _context.SaveChanges();
        }

        public IEnumerable<Exercise> GetAll()
        {
            return _context.Set<Exercise>().ToList();
        }

        public void Delete(int id)
        {
            var exerciseInDb = _context.Exercises.Find(id);
            if (exerciseInDb != null)
            {
                _context.Remove(exerciseInDb);
                _context.SaveChanges();
            }
        }
    }
}
