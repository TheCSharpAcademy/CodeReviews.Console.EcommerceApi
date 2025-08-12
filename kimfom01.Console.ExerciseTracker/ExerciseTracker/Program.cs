using ExerciseTracker.Controller;
using ExerciseTracker.Data;
using ExerciseTracker.Repositories;
using ExerciseTracker.Services;
using ExerciseTracker.UserInput;
using ExerciseTracker.Validations;
using ExerciseTracker.Visualization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<ExerciseContext>(options =>
        {
            options.UseSqlite("Data Source=ExerciseTracker.db");
        });
        services.AddTransient<IExerciseRepository, ExerciseRepository>();
        services.AddTransient<IExerciseService, ExerciseService>();
        services.AddTransient<IExerciseController, ExerciseController>();
        services.AddTransient<IInput, Input>();
        services.AddTransient<IInputValidation, InputValidation>();
        services.AddTransient<ITableVisualization, TableVisualization>();
    })
    .Build();

var serviceProvider = host.Services;
var exerciseController = serviceProvider.GetService<IExerciseController>();

exerciseController!.Run();