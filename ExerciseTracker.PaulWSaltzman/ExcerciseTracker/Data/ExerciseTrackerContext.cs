using Microsoft.EntityFrameworkCore;
using ExerciseTracker.Models;

namespace ExerciseTracker.Data;

public class ExerciseTrackerContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }

    public string DbPath { get; }
    public ExerciseTrackerContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        //users/<user>/AppData/Local on Windows
        DbPath = System.IO.Path.Join(path, "exercise.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

