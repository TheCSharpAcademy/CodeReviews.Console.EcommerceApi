using ExerciseTracker;
using Microsoft.Extensions.DependencyInjection;

var host = Startup.CreateHost(args);

var app = host.Services.GetRequiredService<App>();
await app.Run();


