using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Data;
class ExerciceContext : DbContext
{
    internal DbSet<FieldTours> FieldTours { get; set; }
    internal DbSet<FreeKicks> FreeKicks { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? appFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        int pathLength = appFolderPath.Length - 16;
        string dbFolderPath = appFolderPath.Substring(0, pathLength);
        optionsBuilder.UseSqlite($"Data Source={dbFolderPath}Data/ExerciceTracker.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FieldTours>().HasKey(x => x.Id);
        modelBuilder.Entity<FieldTours>().HasData(
            new FieldTours(1, DateTime.Now.AddDays(-5).AddHours(-5), DateTime.Now.AddDays(-5).AddHours(-4), "First Exercice"),
            new FieldTours(2, DateTime.Now.AddDays(-4).AddHours(-4), DateTime.Now.AddDays(-4).AddHours(-3), "Second Exercice"),
            new FieldTours(3, DateTime.Now.AddDays(-3).AddHours(-5), DateTime.Now.AddDays(-3).AddHours(-4), "Third Exercice"),
            new FieldTours(4, DateTime.Now.AddDays(-2).AddHours(-4), DateTime.Now.AddDays(-2).AddHours(-3), "Fourth Exercice"),
            new FieldTours(5, DateTime.Now.AddDays(-1).AddHours(-5), DateTime.Now.AddDays(-1).AddHours(-4), "Last Exercice"));

        modelBuilder.Entity<FreeKicks>().HasKey(x => x.Id);
        modelBuilder.Entity<FreeKicks>().HasData(
            new FreeKicks(1, DateTime.Now.AddDays(-5).AddHours(-5), DateTime.Now.AddDays(-5).AddHours(-4), "First Exercice"),
            new FreeKicks(2, DateTime.Now.AddDays(-4).AddHours(-4), DateTime.Now.AddDays(-4).AddHours(-3), "Second Exercice"),
            new FreeKicks(3, DateTime.Now.AddDays(-3).AddHours(-5), DateTime.Now.AddDays(-3).AddHours(-4), "Third Exercice"),
            new FreeKicks(4, DateTime.Now.AddDays(-2).AddHours(-4), DateTime.Now.AddDays(-2).AddHours(-3), "Fourth Exercice"),
            new FreeKicks(5, DateTime.Now.AddDays(-1).AddHours(-5), DateTime.Now.AddDays(-1).AddHours(-4), "Last Exercice"));
    }
}