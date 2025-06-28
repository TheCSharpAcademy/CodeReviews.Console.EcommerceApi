using ExerciseTracker.Study.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.HemanthSharma
{
    public class ContextClass : DbContext
    {
        public ContextClass(DbContextOptions options) : base(options) { }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseShift> ExerciseShifts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Exercise>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Exercise>()
                .HasMany(x => x.ExerciseShifts)
                .WithOne(x => x.Exercise)
                .HasForeignKey(x => x.ExerciseId);
            modelBuilder.Entity<ExerciseShift>()
                .HasKey(x => x.Id);
        }
    }
}
