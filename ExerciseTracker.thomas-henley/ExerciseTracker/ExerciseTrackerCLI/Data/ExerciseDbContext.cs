using ExerciseTrackerCLI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTrackerCLI.Data;

public class ExerciseDbContext(DbContextOptions<ExerciseDbContext> options) : DbContext(options)
{
    public DbSet<TreadmillRun> TreadmillRuns { get; set; }
    
}