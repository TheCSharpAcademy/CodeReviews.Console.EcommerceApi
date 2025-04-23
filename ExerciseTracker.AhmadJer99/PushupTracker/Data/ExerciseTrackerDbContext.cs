using Microsoft.EntityFrameworkCore;
using ExerciseTracker.Models;

namespace ExerciseTracker.Data;

public class ExerciseTrackerDbContext : DbContext 
{
    public ExerciseTrackerDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Pushup> Exercises { get; set; }
}