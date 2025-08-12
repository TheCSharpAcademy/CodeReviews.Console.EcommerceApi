using ExerciseTracker.Speedierone.Model;

namespace ExerciseTracker.Speedierone.Repository
{
    public class ExerciseRepository : IExerciseRepository
    {
        public readonly ExerciseDbContext _context;
        public ExerciseRepository(ExerciseDbContext context)
        {
            _context = context;
        }
        public void Add(Exercises exercises)
        {
            _context.Exercises.Add(exercises);
        }
        public void Delete(int id)
        {
            var exerciseToDelete = _context.Exercises.Find(id);
            if (exerciseToDelete != null)
            {
                _context.Exercises.Remove(exerciseToDelete);
            }
            else
            {
                Console.WriteLine("Could not find session matching that ID");
            }
        }

        public IEnumerable<Exercises> GetAll()
        {
            return _context.Exercises.ToList();
        }
        public List<Exercises> GetById(int id)
        {
            return _context.Exercises.Where(exercise => exercise.Id == id).ToList();
        }

        public void Update(Exercises exercisesToUpdate)
        {
            _context.Update(exercisesToUpdate);
            _context.SaveChanges();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
