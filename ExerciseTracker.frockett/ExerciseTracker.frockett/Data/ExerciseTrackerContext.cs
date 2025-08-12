

using ExerciseTracker.frockett.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.frockett.Data;

public class ExerciseTrackerContext : DbContext
{
    public DbSet<ExerciseSession> Sessions { get; set; }

    public ExerciseTrackerContext(DbContextOptions<ExerciseTrackerContext> options) : base(options)
    {
    }

}
