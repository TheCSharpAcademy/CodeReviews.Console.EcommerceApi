namespace ExerciseTracker.Forser.Context
{
    public class ExerciseContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public ExerciseContext(DbContextOptions options) : base(options) { }
    }
}