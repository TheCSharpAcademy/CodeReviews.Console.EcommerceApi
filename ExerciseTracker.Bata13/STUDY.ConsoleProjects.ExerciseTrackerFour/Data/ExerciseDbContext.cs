using Microsoft.EntityFrameworkCore;
using STUDY.ConsoleProjects.ExerciseTrackerFour.Models;

namespace STUDY.ConsoleProjects.ExerciseTrackerFour.Data;
public class ExerciseDbContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\LocalDBDemo;Database=exerciseTrackerDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
