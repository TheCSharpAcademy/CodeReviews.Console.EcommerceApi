using ExerciseTracker.SpyrosZoupas.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.SpyrosZoupas.DAL;

public class ExerciseTrackerDbContext : DbContext
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["exerciseTracker"].ConnectionString;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(connectionString);

    public DbSet<WeightExercise> Exercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeightExercise>()
            .HasData(new List<WeightExercise>
            {
                    new WeightExercise
                    {
                        Id = 1,
                        DateStart = new DateTime(2025, 01, 01, 12, 0, 0),
                        DateEnd = new DateTime(2025, 02, 02, 12, 0, 0),
                        Comments = "My first exercise!",
                        Mass = 5
                    },
                    new WeightExercise
                    {
                        Id = 2,
                        DateStart = new DateTime(2025, 01, 01, 12, 0, 0),
                        DateEnd = new DateTime(2025, 01, 01, 14, 0, 0),
                        Comments = "My second exercise 8)",
                        Mass = 10
                    },
                    new WeightExercise
                    {
                        Id = 3,
                        DateStart = new DateTime(2026, 01, 01, 16, 30, 0),
                        DateEnd = new DateTime(2026, 01, 01, 17, 0, 0),
                        Comments = string.Empty,
                        Mass = 11
                    },
                    new WeightExercise
                    {
                        Id = 4,
                        DateStart = new DateTime(2025, 11, 10, 0, 0, 0),
                        DateEnd = new DateTime(2025, 11, 10, 0, 0 ,50),
                        Comments = "",
                        Mass = 15
                    },
                    new WeightExercise
                    {
                        Id = 5,
                        DateStart = new DateTime(2025, 12, 20),
                        DateEnd = new DateTime(2026, 12, 20),
                        Comments = "My final exercise!",
                        Mass = 20
                    }
            });
    }
}
