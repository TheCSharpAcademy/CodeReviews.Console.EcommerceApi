using ExerciseTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.EF;

public class AppDbContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>().ToTable("Exercises");
    }
}
