using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Data;

public class ExerciseContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }

    public ExerciseContext(DbContextOptions<ExerciseContext> options) : base(options)
    {
    }
}