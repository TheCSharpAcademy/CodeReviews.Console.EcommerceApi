using System.Configuration;
using System.Globalization;
using ExerciseTracker.wkktoria;
using ExerciseTracker.wkktoria.Controllers;
using ExerciseTracker.wkktoria.Data;
using ExerciseTracker.wkktoria.Data.Repositories;
using ExerciseTracker.wkktoria.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        );

        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<IExerciseController, ExerciseController>();
    });

var host = builder.Build();
var serviceProvider = host.Services;

var exerciseService = serviceProvider.GetRequiredService<IExerciseService>();
var exerciseController = new ExerciseController(exerciseService);

var ci = new CultureInfo("en-US");
Thread.CurrentThread.CurrentCulture = ci;
Thread.CurrentThread.CurrentUICulture = ci;

var ui = new UserInput(exerciseController);

ui.Run();