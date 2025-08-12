using ExerciseTracker.Speedierone.Model;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ExerciseTracker.Speedierone
{
    public class ExerciseDbContext : DbContext
    {
        public DbSet<Exercises> Exercises { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
