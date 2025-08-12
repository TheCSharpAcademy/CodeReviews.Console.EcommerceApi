using ExerciseTracker.hasona23.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.hasona23.Data;

public class ExerciseTrackerContext : DbContext
{
       public DbSet<Exercise> Exercises { get ; set; }

       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       {
              optionsBuilder.UseSqlServer(AppSettings.DefaultConnectionString);
              base.OnConfiguring(optionsBuilder);
       }
}