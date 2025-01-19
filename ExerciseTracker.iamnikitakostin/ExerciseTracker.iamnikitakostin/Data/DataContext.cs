using ExerciseTracker.iamnikitakostin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ExerciseTracker.iamnikitakostin.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
   
    public DbSet<Exercise> Exercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Exercise>().HasData(
        new Exercise
        {
            Id = 1,
            DateStart = new DateTime(2025, 1, 10, 8, 0, 0),
            DateEnd = new DateTime(2025, 1, 10, 11, 5, 0),
            Duration = new TimeSpan(3, 5, 0),
            Comments = "Woo Hoo! The first exercise"
        },
        new Exercise
        {
            Id = 2,
            DateStart = new DateTime(2025, 1, 12, 8, 0, 0),
            DateEnd = new DateTime(2025, 1, 12, 9, 1, 0),
            Duration = new TimeSpan(1, 1, 0),
            Comments = "Woo Hoo! The second exercise"
        },
        new Exercise
        {
            Id = 3,
            DateStart = new DateTime(2025, 1, 18, 12, 0, 0),
            DateEnd = new DateTime(2025, 1, 18, 14, 30, 0),
            Duration = new TimeSpan(1, 30, 0),
            Comments = "Woo Hoo! The third exercise"
        }
        );
    }
}

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        var connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);

        return new DataContext(optionsBuilder.Options);
    }
}