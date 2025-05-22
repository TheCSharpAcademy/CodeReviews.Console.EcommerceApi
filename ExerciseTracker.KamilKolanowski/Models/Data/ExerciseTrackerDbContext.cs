using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.KamilKolanowski.Models.Data;

public class ExerciseTrackerDbContext : DbContext
{
    public ExerciseTrackerDbContext(DbContextOptions<ExerciseTrackerDbContext> options)
        : base(options) { }

    public DbSet<Exercise> Exercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("TCSA");
        base.OnModelCreating(modelBuilder);
    }
}
