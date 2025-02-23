using ExerciseTracker.Dejmenek.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ExerciseTracker.Dejmenek.Data;
public class ExerciseContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["LocalDbConnection"].ConnectionString);
    }

}
