using Microsoft.EntityFrameworkCore;
using ExerciseTrackerCarDioLogics.Models;

namespace ExerciseTrackerCarDioLogics.Data;

public class SessionContext : DbContext
{
    public DbSet<Session> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ExerciseTrackerDB;Trusted_Connection=True;");
    }
}
