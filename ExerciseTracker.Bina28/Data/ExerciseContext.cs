using ExerciseTracker.Models;

namespace ExerciseTracker.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
internal class ExerciseContext : DbContext
{
	private readonly string _connectionString;

	// Inject IConfiguration through the constructor
	public ExerciseContext(IConfiguration configuration)
	{
		_connectionString = configuration.GetConnectionString("DefaultConnection") ??
							throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
	}

	public DbSet<ExerciseModel> Exercise { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(_connectionString);
	}
}


