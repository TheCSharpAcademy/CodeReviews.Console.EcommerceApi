using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sadklouds.ExcerciseTracker.Controllers;
using sadklouds.ExcerciseTracker.DataInput;
using sadklouds.ExcerciseTracker.DBContext;
using sadklouds.ExcerciseTracker.Repositries;
using sadklouds.ExcerciseTracker.Services;

IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
IHost _host = Host.CreateDefaultBuilder().ConfigureServices(
    services =>
    {
        services.AddDbContext<ExerciseContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("Default"));
        });
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IUserInput, UserInput>();
        services.AddScoped<IExerciseController, ExerciseController>();
    }).Build();

var exerciseController = _host.Services.GetService<IExerciseController>();
exerciseController!.Run();

