using Exercise_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercise_Tracker.Data;

public class ExerciseDbContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=exercise_db;Trusted_Connection=True"
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Exercise>()
            .HasData(
                new Exercise
                {
                    Id = 1,
                    StartTime = new DateTime(2025, 7, 15, 8, 0, 0),
                    EndTime = new DateTime(2025, 7, 15, 8, 45, 0),
                    Comments = "Morning workout session.",
                },
                new Exercise
                {
                    Id = 2,
                    StartTime = new DateTime(2025, 7, 16, 13, 0, 0),
                    EndTime = new DateTime(2025, 7, 16, 14, 0, 0),
                    Comments = "Afternoon workout session.",
                },
                new Exercise
                {
                    Id = 3,
                    StartTime = new DateTime(2025, 7, 17, 21, 0, 0),
                    EndTime = new DateTime(2025, 7, 17, 21, 30, 0),
                    Comments = "Evening workout session.",
                },
                new Exercise
                {
                    Id = 4,
                    StartTime = new DateTime(2025, 7, 18, 21, 0, 0),
                    EndTime = new DateTime(2025, 7, 18, 21, 30, 0),
                    Comments = "Evening running session.",
                },
                new Exercise
                {
                    Id = 5,
                    StartTime = new DateTime(2025, 7, 19, 16, 15, 0),
                    EndTime = new DateTime(2025, 7, 19, 16, 45, 0),
                    Comments = "Afternoon swimming session.",
                }
            );
    }
}
