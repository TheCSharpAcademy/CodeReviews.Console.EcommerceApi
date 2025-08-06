using ExerciseTracker.Niasua.Controllers;
using ExerciseTracker.Niasua.Data;
using ExerciseTracker.Niasua.Repositories;
using ExerciseTracker.Niasua.Services;
using ExerciseTracker.Niasua.UI.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(config);

services.AddDbContext<ExerciseContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

services.AddScoped<IExerciseRepository, ExerciseRepository>();
services.AddScoped<ExerciseService>();
services.AddScoped<ExerciseController>();

var serviceProvider = services.BuildServiceProvider();

var controller = serviceProvider.GetRequiredService<ExerciseController>();

await MainMenu.Show(controller);

