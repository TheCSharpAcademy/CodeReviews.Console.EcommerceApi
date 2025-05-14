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
                        DateStart = new DateTime(2025, 01, 01, 12, 0, 0),
                        DateEnd = new DateTime(2025, 02, 02, 12, 0, 0),
                        Comments = "My first exercise!"
                    },
                    new Exercise
                    {
                        ExerciseId = 2,
                        DateStart = new DateTime(2025, 01, 01, 12, 0, 0),
                        DateEnd = new DateTime(2025, 01, 01, 14, 0, 0),
                        Comments = "My second exercise 8)"
                    },
                    new Exercise
                    {
                        ExerciseId = 3,
                        DateStart = new DateTime(2026, 01, 01, 16, 30, 0),
                        DateEnd = new DateTime(2026, 01, 01, 17, 0, 0),
                        Comments = string.Empty
                    },
                    new Exercise
                    {
                        ExerciseId = 4,
                        DateStart = new DateTime(2025, 11, 10, 0, 0, 0),
                        DateEnd = new DateTime(2025, 11, 10, 0, 0 ,50),
                        Comments = ""
                    },
                    new Exercise
                    {
                        ExerciseId = 5,
                        DateStart = new DateTime(2025, 12, 20),
                        DateEnd = new DateTime(2026, 12, 20),
                        Comments = "My final exercise!"
                    }
            });
    }
}
