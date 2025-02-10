using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExerciseTracker.Controllers;
using ExerciseTracker.Repositories;
using ExerciseTracker.UI;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IRepository, ExerciseRepository>();
builder.Services.AddTransient<IController, ExerciseController>();
builder.Services.AddTransient<UserInput>();

using IHost host = builder.Build();

var serviceProvider = host.Services;
var exerciseController = serviceProvider.GetRequiredService<IController>();
exerciseController.Menu();
host.Run();
