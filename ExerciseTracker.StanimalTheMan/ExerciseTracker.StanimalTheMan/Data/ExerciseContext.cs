using ExerciseTracker.StanimalTheMan.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.StanimalTheMan.Data;

public class ExerciseContext : DbContext
{
	public ExerciseContext(DbContextOptions<ExerciseContext> options) : base(options)
	{
	}

	public DbSet<Run> RunEntries { get; set; }
}
