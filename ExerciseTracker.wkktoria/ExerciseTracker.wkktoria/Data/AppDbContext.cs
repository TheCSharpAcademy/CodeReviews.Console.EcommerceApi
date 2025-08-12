using ExerciseTracker.wkktoria.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.wkktoria.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Exercise> Exercises { get; set; } = null!;
}