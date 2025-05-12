using ExerciseTracker.Brozda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ExerciseTracker.Brozda.Data
{
    internal class ExcerciseTrackerContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection_string = @"Data Source=(localdb)\LOCALDB;Initial Catalog=ExcerciseTracker;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            optionsBuilder.UseSqlServer(connection_string)
                /*.LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()*/
                .UseSeeding((dbContext, _) =>
                {
                    var projectRoot = Environment.GetEnvironmentVariable("PROJECT_ROOT");

                    var path = Path.Combine(projectRoot ?? Directory.GetCurrentDirectory(), "Resources", "SeedData.json");

                    if (File.Exists(path))
                    {
                        var rawData = File.ReadAllText(path);
                        var deserialized = JsonSerializer.Deserialize<SeedData>(rawData);

                        if (deserialized is not null)
                        {
                            foreach(var excercise  in deserialized.Exercises)
                            {
                                excercise.Duration = (long)(excercise.DateEnd - excercise.DateStart).TotalSeconds;
                            }


                            dbContext.AddRange(deserialized.Exercises);
                            dbContext.SaveChanges();
                        }
                    }
                });

        }
 
    }
}
