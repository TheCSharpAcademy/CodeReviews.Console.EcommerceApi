using App.Controllers;
using App.Database;
using App.Database.EntityFramework;
using App.ExerciseLogs;
using App.ExerciseLogs.Models;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace App;

public class Program
{
    public static void Main()
    {
        var serviceProvider = BuildServiceProvider();

        var userInterface = serviceProvider?.GetService<AppController>();

        if (userInterface is null)
        {
            AnsiConsole.WriteLine("App failed to start");
            Environment.Exit(1);
        }

        userInterface.Run().GetAwaiter().GetResult();
    }

    public static ServiceProvider BuildServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<ExercisesDbContext>();
        serviceCollection.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        serviceCollection.AddTransient<IRepositoryBase<ExerciseLog>, RepositoryBase<ExerciseLog>>();
        serviceCollection.AddTransient(typeof(ExerciseService));
        serviceCollection.AddTransient(typeof(AppController));
        serviceCollection.AddTransient(typeof(ExerciseController));

        return serviceCollection.BuildServiceProvider();
    }
}