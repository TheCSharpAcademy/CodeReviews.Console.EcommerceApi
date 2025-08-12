using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ExerciseTracker.Data;

public class ExerciseContext : DbContext
{
    internal DbSet<Exercise> Exercises { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = ConfigurationManager.AppSettings.Get("DbPath");
        optionsBuilder.UseSqlServer(dbPath);
    }
}
