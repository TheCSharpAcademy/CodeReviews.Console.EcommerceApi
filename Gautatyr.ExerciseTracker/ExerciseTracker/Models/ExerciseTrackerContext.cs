using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Models;

public class ExerciseTrackerContext : DbContext
{
    public ExerciseTrackerContext()
    {
    }

    public DbSet<Run> Run { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("ExerciseTracker.db");
}