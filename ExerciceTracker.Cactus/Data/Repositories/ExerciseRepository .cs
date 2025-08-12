using ExerciseTracker.Cactus.Data.Interfaces;
using ExerciseTracker.Cactus.Model;

namespace ExerciseTracker.Cactus.Data.Repositories
{
    public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(ExerciseDbContext context) : base(context)
        {
        }
    }
}
