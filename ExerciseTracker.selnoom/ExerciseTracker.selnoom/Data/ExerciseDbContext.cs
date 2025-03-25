using ExerciseTracker.selnoom.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Data
{
    public class ExerciseDbContext : DbContext
    {
        public ExerciseDbContext(DbContextOptions<ExerciseDbContext> options)
            : base(options)
        {
        }

        public DbSet<WeightExercise> Exercises { get; set; }
    }
}
