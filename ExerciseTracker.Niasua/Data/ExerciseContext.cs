using ExerciseTracker.Niasua.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Niasua.Data;

public class ExerciseContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }
    public ExerciseContext(DbContextOptions<ExerciseContext> options) : base(options)
    {
    }
}
