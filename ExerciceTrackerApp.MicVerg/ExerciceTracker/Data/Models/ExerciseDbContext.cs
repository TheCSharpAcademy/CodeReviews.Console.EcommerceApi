using Microsoft.EntityFrameworkCore;

namespace ExerciceTracker.Data.Models
{
    internal class ExerciseDbContext : DbContext
    {
        public ExerciseDbContext(DbContextOptions<ExerciseDbContext> options) : base(options)
        {
        }

        public DbSet<Exercise> Exercises { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-ETA4JL7;Database=Exercices;Trusted_Connection=True;Integrated Security=True;Encrypt=False;");
        }
    }
}
