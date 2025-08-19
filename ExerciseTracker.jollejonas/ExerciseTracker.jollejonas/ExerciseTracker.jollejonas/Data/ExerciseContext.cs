using ExerciseTracker.jollejonas.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.jollejonas.Data;
public class ExerciseContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = ExerciseTrackerContext; Trusted_Connection = True; MultipleActiveResultSets = true");
    }
}