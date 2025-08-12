using ExerciseTracker.Cactus.Model;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Cactus.Data
{
    public class ExerciseDbContext : DbContext
    {
        public DbSet<Exercise> ExerciseSet { get; set; }

        public ExerciseDbContext(DbContextOptions<ExerciseDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
