using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ExerciseTracker.KroksasC.Models
{
    internal class ExerciseTrackerDbContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ExerciseTracker"].ConnectionString);
        }
    }
}
