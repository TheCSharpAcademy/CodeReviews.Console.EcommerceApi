using Exercise_Tracker.Interfaces;
using Exercise_Tracker.Model;

namespace Exercise_Tracker.Repository
{
    public class ExcerciseRepository : IExcerciseRepository
    {
        private readonly ExerciseTrackerContext _context;

        public ExcerciseRepository(ExerciseTrackerContext context)
        {
            _context = context;
        }

        public void AddRegistry( Exercise exercise )
        {
            _context.Add(exercise);
            _context.SaveChanges();
        }

        public IEnumerable<Exercise> GetAll()
        {
            return _context.Set<Exercise>().ToList();
        }

        public void RemoveRegistry( int id )
        {
            var dbAcess = _context.Set<Exercise>().Find(id);
            if (dbAcess != null)
            {
                _context.Remove(dbAcess);
                _context.SaveChanges();
            }
        }

        public Exercise SearchById( int id )
        {
            return _context.Find<Exercise>(id);
        }

        public void UpdateRegistry( Exercise exercise )
        {
            _context.Update(exercise);
            _context.SaveChanges();
        }
    }
}
