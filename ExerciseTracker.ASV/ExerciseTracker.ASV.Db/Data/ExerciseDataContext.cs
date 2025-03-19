using Microsoft.EntityFrameworkCore;
using ExerciseTracker.ASV.Db.Models;

namespace ExerciseTracker.ASV.Db.Data;

public class ExerciseDataContext : DbContext
{
    public DbSet<ExerciseData> Exercise { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        DotNetEnv.Env.TraversePath().Load();
        optionsBuilder.UseSqlServer(DotNetEnv.Env.GetString("CONNECTION_STRING"));
    }
}