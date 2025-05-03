using Microsoft.EntityFrameworkCore;

public class ExerciseContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"
            Server=localhost;
            Database=exercisetrackerdb;
            User Id=sa;
            Password=StrongP@ssword1;
            TrustServerCertificate=True
        ");
    }
}