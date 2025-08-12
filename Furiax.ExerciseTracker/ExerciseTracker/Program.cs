using ExerciseTracker;
using ExerciseTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
	.ConfigureServices((hostContext, services) =>
	{
		services.AddDbContext<ExerciseTrackerContext>(options =>
			options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=ExerciseTracker"));
		services.AddTransient<IExerciseRepository, ExerciseRepository>();
		services.AddTransient<ExerciseService>();
		services.AddTransient<ExerciseController>();
	})
	.Build();

using (var scope = host.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var context = services.GetRequiredService<ExerciseTrackerContext>();
	var exerciseController = services.GetRequiredService<ExerciseController>();
	exerciseController.MainMenu();
}