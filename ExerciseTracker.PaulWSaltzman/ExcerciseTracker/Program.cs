using ExerciseTracker.Controllers;
using ExerciseTracker.Data;
using ExerciseTracker.Repositories;
using ExerciseTracker.Services;
using ExerciseTracker.UserInterface;
using Microsoft.Extensions.DependencyInjection;



namespace ExerciseTracker;

public class Program
{
    public static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<ExerciseTrackerContext>()
            .AddScoped<IExerciseRepository, ExerciseRepository>()
            .AddScoped<IExerciseService, ExerciseService>()
            .AddScoped<ExerciseController>()
            .BuildServiceProvider();

        var controller = serviceProvider.GetService<ExerciseController>();

        if (controller != null)
        {
            var menu = new Menu(controller);
            menu.MainMenu();
        }
        else
        {
            Console.WriteLine("Failed to initialize controller.");
        }
    }
}
