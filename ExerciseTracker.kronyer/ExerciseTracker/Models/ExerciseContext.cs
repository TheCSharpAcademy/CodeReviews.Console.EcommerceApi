using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Models;

internal class ExerciseContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=exercise-tracker.db");
    }
}
