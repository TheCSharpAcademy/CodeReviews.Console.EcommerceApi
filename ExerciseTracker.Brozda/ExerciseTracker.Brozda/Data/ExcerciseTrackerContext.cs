using ExerciseTracker.Brozda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExerciseTracker.Brozda.Data
{
    internal class ExcerciseTrackerContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection_string = @"Data Source=(localdb)\LOCALDB;Initial Catalog=ExcerciseTracker;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            optionsBuilder.UseSqlServer(connection_string)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    }
}
