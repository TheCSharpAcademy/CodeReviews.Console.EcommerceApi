using ExerciseTracker.kjanos89.Controllers;
using ExerciseTracker.kjanos89.Models;
using ExerciseTracker.kjanos89.Repository;
using ExerciseTracker.kjanos89.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
public static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Loading, please wait.");
        var serviceProvider = new ServiceCollection()
        .AddDbContext<ExerciseDbContext>(options =>
         options.UseSqlServer("Server=localhost;Database=Exercises;Integrated Security=True;TrustServerCertificate=True;"))
        .AddScoped<IExerciseRepository, ExerciseRepository>()
        .AddScoped<Service>()
        .AddScoped<Controller>()
        .BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<ExerciseDbContext>();
        context.Database.EnsureCreated();

        var controller = serviceProvider.GetRequiredService<Controller>();
        controller.ShowMenu();
    }
}