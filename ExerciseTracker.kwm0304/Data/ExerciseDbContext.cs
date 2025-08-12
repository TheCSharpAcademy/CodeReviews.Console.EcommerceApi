using ExerciseTracker.kwm0304.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.kwm0304.Data;

public class ExerciseDbContext : DbContext
{
  public ExerciseDbContext(DbContextOptions<ExerciseDbContext> options) : base(options)
  {
  }

  public DbSet<UserInput> UserInputs { get; set; } = null!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
      optionsBuilder.UseSqlServer(connectionString);
    }
  }
}