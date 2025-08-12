using ExerciseTracker.StevieTV.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.StevieTV.Database;

public class ExerciseContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }

    public ExerciseContext(DbContextOptions<ExerciseContext> options) : base(options)
    {
    }

}
    