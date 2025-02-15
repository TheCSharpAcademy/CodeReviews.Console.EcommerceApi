using ExerciseTracker.Controllers;
using ExerciseTracker.Data;
using ExerciseTracker.Repositories;
using ExerciseTracker.Services;
using ExerciseTracker.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("ExerciseTrackerConnection");
builder.Services.AddDbContext<ExerciseDbContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddTransient<IRepository, ExerciseRepository>();
builder.Services.AddTransient<IController, ExerciseController>();
builder.Services.AddTransient<IService, ExerciseService>();
builder.Services.AddSingleton<UserInput>();

using IHost host = builder.Build();

var serviceProvider = host.Services;
var exerciseController = serviceProvider.GetRequiredService<IController>();
exerciseController.Menu();
host.Run();