using ExerciseTracker.samggannon.Controllers;
using ExerciseTracker.samggannon.Data.Repositories;
using ExerciseTracker.samggannon.Services;
using ExerciseTracker.samggannon.UserInterface;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddScoped<ExerciseRepository>()
    .AddScoped<ResistanceRespository>()
    .AddScoped<IExerciseService, ExerciseService>()
    .AddScoped<ExerciseController>()
    .BuildServiceProvider();

var mainMenu = new MainMenu(serviceProvider);
mainMenu.ShowMenu();

