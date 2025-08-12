using GymExerciseTracker.Controllers;
using GymExerciseTracker.Data;
using GymExerciseTracker.Repository;
using GymExerciseTracker.Services;
using GymExerciseTracker.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Register your services here.
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer("Server=.;Database=GymTrackerDb;Trusted_Connection=True;TrustServerCertificate=True");
        });
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<ExerciseController>();
        services.AddLogging();
    });

var host = builder.Build();

var serviceProvider = host.Services;

// Retrieve Exercise service from the DI container.
var exerciseService = serviceProvider.GetRequiredService<IExerciseService>();

ExerciseController controller = new ExerciseController(exerciseService);

UserInput userInput = new UserInput(controller);
userInput.Start();