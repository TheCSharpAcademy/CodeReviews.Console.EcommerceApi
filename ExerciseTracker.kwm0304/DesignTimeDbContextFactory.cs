using dotenv.net;
using ExerciseTracker.kwm0304.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExerciseTracker.kwm0304;
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ExerciseDbContext>
{
  public ExerciseDbContext CreateDbContext(string[] args)
  {
    DotEnv.Load();

    var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

    var optionsBuilder = new DbContextOptionsBuilder<ExerciseDbContext>();
    optionsBuilder.UseSqlServer(connectionString);

    return new ExerciseDbContext(optionsBuilder.Options);
  }
}
