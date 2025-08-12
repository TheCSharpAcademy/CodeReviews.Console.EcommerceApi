using Kmakai.ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;


namespace Kmakai.ExerciseTracker.DataAccess;

public class ExerciseContext: DbContext
{
    public ExerciseContext(DbContextOptions<ExerciseContext> options)
        : base(options)
    {
    }

    public DbSet<Exercise> Exercises { get; set; } = null!;

}
