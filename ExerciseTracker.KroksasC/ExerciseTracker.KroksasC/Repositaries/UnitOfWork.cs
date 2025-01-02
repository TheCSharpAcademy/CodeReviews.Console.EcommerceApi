using ExerciseTracker.KroksasC.Models;

namespace ExerciseTracker.KroksasC.Repositaries
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ExerciseTrackerDbContext _context;
        public IRepository<Exercise> Exercises { get; private set; }

        public UnitOfWork(ExerciseTrackerDbContext context)
        {
            _context = context;
            Exercises = new Repository<Exercise>(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
