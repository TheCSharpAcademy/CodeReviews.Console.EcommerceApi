using ExerciseTracker.Console.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Console.Data;

public class ExerciseContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }
    public ExerciseContext(DbContextOptions<ExerciseContext> options) : base(options)
    {
    }
}
