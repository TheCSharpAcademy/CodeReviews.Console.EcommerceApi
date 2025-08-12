using Microsoft.EntityFrameworkCore;
using ExerciseTracker.Arashi256.Config;

namespace ExerciseTracker.Arashi256.Models
{
    public class ExerciseDbContext : DbContext
    {
        private readonly AppSettings _connection;

        public ExerciseDbContext(AppSettings connection)
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connection.DatabaseConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExerciseSession>().HasKey(es => es.Id);
        }

        public DbSet<ExerciseSession> ExerciseSessions { get; set; }
    }
}