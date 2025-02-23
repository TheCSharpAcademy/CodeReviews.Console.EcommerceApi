using Microsoft.EntityFrameworkCore;
using ExerciseTracker.Models;
using ExerciseTracker.Controller;

namespace ExerciseTracker.Data;

internal class ExerciseContext : DbContext
{
    private readonly string _connectionString = Helpers.GetConnectionString();

    public ExerciseContext() : base() { }

    public ExerciseContext(DbContextOptions<ExerciseContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }

    public DbSet<Exercise> Exercises { get; set; }
}
