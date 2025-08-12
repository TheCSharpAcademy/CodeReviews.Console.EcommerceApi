using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker;
public class ExerciseDatabase : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    }
    public DbSet<Exercise> Exercise { get; set; }
}