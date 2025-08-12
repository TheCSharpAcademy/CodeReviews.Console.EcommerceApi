using ExerciseTracker.StanimalTheMan;
using ExerciseTracker.StanimalTheMan.Controllers;
using ExerciseTracker.StanimalTheMan.Data;
using ExerciseTracker.StanimalTheMan.Repository;
using ExerciseTracker.StanimalTheMan.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

class Program
{
	static void Main(string[] args)
	{
		var connectionString = "Server=(LocalDb)\\LocalDBDemo;Database=RunEntries;Integrated Security=True;Encrypt=False";


		var serviceProvider = new ServiceCollection()
			.AddDbContext<ExerciseContext>(options =>
				options.UseSqlServer(connectionString))
			.AddTransient<IExerciseRepository, ExerciseRepository>()
			.AddTransient<ExerciseService>()
			.AddTransient<ExerciseController>()
			.BuildServiceProvider();

		using (var serviceScope = serviceProvider.CreateScope())
		{
			var dbContext = serviceScope.ServiceProvider.GetRequiredService<ExerciseContext>();
			dbContext.Database.EnsureCreated();
		}

		var exerciseController = serviceProvider.GetService<ExerciseController>();

		if (exerciseController != null)
		{
			UserInput.ShowMainMenu(exerciseController);
		}
		else
		{
			Console.WriteLine("Failed to retrieve ExerciseController.");
		}
	}

}
