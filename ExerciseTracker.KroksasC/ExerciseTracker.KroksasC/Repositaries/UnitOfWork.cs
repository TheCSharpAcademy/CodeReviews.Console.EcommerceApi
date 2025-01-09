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
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
