using ExerciseTracker.tonyissa.Models;
using ExerciseTracker.tonyissa.Repositories;
using ExerciseTracker.tonyissa.Services;
using ExerciseTracker.tonyissa.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ExerciseContext>(opt => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    opt.UseSqlServer(connectionString);
});

builder.Services.AddTransient<IExerciseRepository<ExerciseSession>, ExerciseRepository<ExerciseSession>>();
builder.Services.AddTransient<ExerciseService>();
builder.Services.AddTransient<MainMenuController>();

var host = builder.Build();

var app = host.Services.GetRequiredService<MainMenuController>();

while (true)
{
    try
    {
        app.StartMainMenu();
        return;
    }
    catch (Exception ex)
    {
        AnsiConsole.WriteException(ex);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}