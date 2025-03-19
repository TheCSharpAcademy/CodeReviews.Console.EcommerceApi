using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExerciseTracker.ASV.Controllers;
using ExerciseTracker.ASV.Services;
using ExerciseTracker.ASV;
using ExerciseTracker.ASV.UserInput;
using ExerciseTracker.ASV.Repositories;
using ExerciseTracker.ASV.Views;

IConfiguration configuration = new ConfigurationBuilder().Build();

var services = new ServiceCollection();

services.AddSingleton(configuration);
services.AddSingleton<IStartup, Startup>();
services.AddSingleton<HttpClient>();
services.AddTransient<IExerciseService, ExerciseService>();
services.AddTransient<IExerciseController, ExerciseController>();
services.AddTransient<IInput,  Input>();
services.AddTransient<IExerciseRepository, ExerciseRepository>();
services.AddTransient<IDisplay, Display>();

var serviceProvider = services.BuildServiceProvider();
var startup = serviceProvider.GetService<IStartup>();

await startup!.Run();