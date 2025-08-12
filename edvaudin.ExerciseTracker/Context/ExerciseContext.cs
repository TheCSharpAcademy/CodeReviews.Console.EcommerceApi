using edvaudin.ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace edvaudin.ExerciseTracker.Context;

public class ExerciseContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }

    public ExerciseContext(DbContextOptions options) : base(options) { }
}
