using ExerciseTracker.Brozda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace ExerciseTracker.Brozda.Data
{
    /// <summary>
    /// Represent database context for EF core for Excercise Tracker application
    /// </summary>
    /// <remarks>
    /// Context uses MSSQL server. On configuration it will automatically seed data from SeedData.json located in Resources folder within root folder (if present). 
    /// This can be overriden by setting env. variable "PROJECT_ROOT" to different location
    /// </remarks>
    internal class ExerciseTrackerContext : DbContext
    {
        public DbSet<Exercise> ExercisesWeight { get; set; } = null!;
        public DbSet<Exercise> ExercisesCardio { get; set; } = null!;
        public DbSet<ExerciseType> ExerciseTypes { get; set; } = null!;
        /// <summary>
        /// Configures the database provider and optionally seeds data from a JSON file.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection_string = @"Data Source=(localdb)\LOCALDB;Initial Catalog=ExcerciseTracker;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            optionsBuilder.UseSqlServer(connection_string)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseSeeding((dbContext, _) =>
                {
                    var projectRoot = Environment.GetEnvironmentVariable("PROJECT_ROOT");

                    var path = Path.Combine(projectRoot ?? Directory.GetCurrentDirectory(), "Resources", "SeedData.json");

                    

                    AutoSeedHelper(dbContext, path);   
                });

        }
        /// <summary>
        /// Static method managing autoseed functionality
        /// </summary>
        /// <param name="context">DB context used for DB access</param>
        /// <param name="jsonPath">string path to JSON file</param>
        static void AutoSeedHelper(DbContext context, string jsonPath)
        {
            if (File.Exists(jsonPath))
            {
                var rawData = File.ReadAllText(jsonPath);
                var seedData = JsonSerializer.Deserialize<SeedDataEf>(rawData);

                if (seedData is not null)
                {
                    foreach (var excercise in seedData.ExercisesWeight)
                    {
                        excercise.Duration = (long)(excercise.DateEnd - excercise.DateStart).TotalSeconds;
                    }

                    context.Set<ExerciseType>().AddRange(seedData.ExerciseTypes);
                    context.SaveChanges();

                    context.Set<Exercise>().AddRange(seedData.ExercisesWeight);
                    context.SaveChanges();
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exercise>().ToTable("ExercisesWeight");
        }

    }
}
