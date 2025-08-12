using ExerciseTracker;
using ExerciseTracker.Controller;
using ExerciseTracker.Input;
using ExerciseTracker.Repository;
using ExerciseTracker.Service;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
ExerciseContext context = new(configuration);
ExerciseRepository repository = new(context);
ExerciseService service = new(repository);
UserInput input = new();
ExerciseController exerciseController = new(service, input);

await exerciseController.MainMenu();