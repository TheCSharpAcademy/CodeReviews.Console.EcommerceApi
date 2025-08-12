using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ExerciseTracker.Models;
public class ExerciseTrackerContext(DbContextOptions<ExerciseTrackerContext> options) : DbContext(options)
{
    public const int CommentsLength = 200;

    public DbSet<Running> RunningExercise {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Running>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Running>()
            .Property(p => p.Id)
            .UseIdentityColumn();

        modelBuilder.Entity<Running>()
            .Property( p => p.Duration)
            .HasConversion(new TimeSpanToTicksConverter());

        modelBuilder.Entity<Running>()
            .Property(p => p.Comments)
            .HasMaxLength(CommentsLength)
            .IsUnicode(true);  
    }
}