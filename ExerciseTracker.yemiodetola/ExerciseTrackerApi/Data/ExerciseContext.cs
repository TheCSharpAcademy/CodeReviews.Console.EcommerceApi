using Microsoft.EntityFrameworkCore;
using ExerciseTrackerApi.Models;

namespace ExerciseTrackerApi.Data;

public class ExerciseContext : DbContext
{
  public ExerciseContext(DbContextOptions<ExerciseContext> options) : base(options)
  {
  }
  public DbSet<Exercise> Exercises { get; set; } = null!;
}
