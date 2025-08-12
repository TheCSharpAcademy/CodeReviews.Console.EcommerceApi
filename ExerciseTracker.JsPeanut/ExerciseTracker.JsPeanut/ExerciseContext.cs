using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.JsPeanut
{
    public class ExerciseContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
         optionsBuilder.UseSqlServer("Data Source=(localdb)\\LocalDBDemo;Initial Catalog=Exercises;Integrated Security=True;Connect Timeout=30;Encrypt=False");
    }
}
