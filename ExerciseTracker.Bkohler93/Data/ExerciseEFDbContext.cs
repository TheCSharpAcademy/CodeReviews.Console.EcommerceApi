using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ExerciseEFDbContext : DbContext
{
    public DbSet<Exercise> Runs { get; set; }
    public ExerciseEFDbContext(DbContextOptions<ExerciseEFDbContext> options):base(options)
    {
        
    }
}
