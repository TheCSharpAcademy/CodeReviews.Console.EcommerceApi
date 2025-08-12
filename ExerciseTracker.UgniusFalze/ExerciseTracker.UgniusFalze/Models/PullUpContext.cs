using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.UgniusFalze.Models;

public class PullUpContext:DbContext
{
    public PullUpContext(DbContextOptions<PullUpContext> options)
        : base(options)
    {
        
    }

    public DbSet<Pullup> Pullups { get; set; }
}