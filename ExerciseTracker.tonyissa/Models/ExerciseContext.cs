using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.tonyissa.Models;

public class ExerciseContext(DbContextOptions<ExerciseContext> options) : DbContext(options)
{
    public DbSet<ExerciseSession> Sessions { get; set; }
}