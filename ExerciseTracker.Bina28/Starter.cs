using ExerciseTracker.Controller;
using ExerciseTracker.Data;
using ExerciseTracker.Models;
using ExerciseTracker.Repositories;
using ExerciseTracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExerciseTracker;

internal class Starter
{
	public static ServiceProvider InitializeServices()
	{
		var serviceProvider = new ServiceCollection()
			.AddSingleton<IConfiguration>(new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build())
			.AddScoped<IExerciseRepository<ExerciseModel>, ExerciseRepository<ExerciseModel>>()
			.AddScoped<ExerciseServices>()
			.AddScoped<ExerciseController>()
			.AddScoped<UserInput>()
			.AddDbContext<ExerciseContext>(options =>
				options.UseSqlServer("DefaultConnection"))
			.BuildServiceProvider();

		return serviceProvider;
	}
}
