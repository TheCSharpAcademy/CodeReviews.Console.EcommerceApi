using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ExerciseTracker.samggannon.Data.Models;

internal class ExerciseContext : DbContext
{
    public DbSet<Exercise> ExerciseSet { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString);
        }
    }
}
