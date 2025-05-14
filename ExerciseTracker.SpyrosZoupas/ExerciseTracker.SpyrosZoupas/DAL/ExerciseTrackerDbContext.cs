using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.SpyrosZoupas.DAL;

public class ExerciseTrackerDbContext : DbContext
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["exerciseTracker"].ConnectionString;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(connectionString);

    public DbSet<Exercise> Exercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>()
            .HasData(new List<Exercise>
            {
                    new Exercise
                    {
                        ExerciseId = 1,
                        DateStart = new DateTime(2025, 01, 01),
                        DateEnd = new DateTime(2025, 02, 01),
                        Comments = "My first exercise!"
                    },
                    new Exercise
                    {
                        ExerciseId = 2,
                        DateStart = new DateTime(2025, 01, 01),
                        DateEnd = new DateTime(2025, 01, 05),
                        Comments = "My second exercise 8)"
                    },
                    new Exercise
                    {
                        ExerciseId = 3,
                        DateStart = new DateTime(2025, 01, 01),
                        DateEnd = new DateTime(2026, 01, 01),
                        Comments = string.Empty
                    },
                    new Exercise
                    {
                        ExerciseId = 4,
                        DateStart = new DateTime(2025, 11, 10),
                        DateEnd = new DateTime(2025, 12, 20)
                    },
                    new Exercise
                    {
                        ExerciseId = 5,
                        DateStart = new DateTime(2025, 11, 10),
                        DateEnd = new DateTime(2025, 12, 20),
                        Comments = "My final exercise!"
                    }
            });
    }
}
