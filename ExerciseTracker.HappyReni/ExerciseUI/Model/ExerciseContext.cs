using Microsoft.EntityFrameworkCore;

namespace ExerciseUI.Model
{
    public class ExerciseContext : DbContext
    {
        public ExerciseContext() { }

        public ExerciseContext(DbContextOptions<ExerciseContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Exercises;Integrated Security=true");
        }

        public DbSet<ExerciseModel> Exercises { get; set; } = null!;
    }
}
