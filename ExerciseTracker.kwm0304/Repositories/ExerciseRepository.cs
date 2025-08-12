using ExerciseTracker.kwm0304.Data;
using ExerciseTracker.kwm0304.Models;

namespace ExerciseTracker.kwm0304.Repositories;

public class ExerciseRepository : Repository<UserInput>
{
    private readonly ExerciseDbContext _context;
    public ExerciseRepository(ExerciseDbContext context) : base(context)
    {
      _context = context;
    }
}