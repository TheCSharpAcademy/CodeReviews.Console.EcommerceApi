using exerciseTracker.doc415.Models;
using Microsoft.EntityFrameworkCore;
namespace exerciseTracker.doc415.context;

internal class ExerciseDbContext : DbContext
{
    private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=Exercise; Integrated Security=true;";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Data source to be replaced with config connection string.
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>()
            .HasKey(Exercise => new { Exercise.Id });
    }
    public DbSet<Exercise> Exercies { get; set; }

}
