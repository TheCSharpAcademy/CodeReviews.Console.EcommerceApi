using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker
{
	public class ExerciseTrackerContext : DbContext
	{
        public ExerciseTrackerContext(DbContextOptions<ExerciseTrackerContext>options) 
			: base(options)
        {  
        }
		public DbSet<ExerciseModel> Exercises { get; set; }
	
	}
}
