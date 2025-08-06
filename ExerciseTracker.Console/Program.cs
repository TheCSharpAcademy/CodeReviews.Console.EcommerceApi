using ExerciseTracker.Niasua.Controllers;
using ExerciseTracker.Niasua.Data;
using ExerciseTracker.Niasua.Repositories;
using ExerciseTracker.Niasua.Services;
using ExerciseTracker.Niasua.UI.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("DefaultConnection");

var optionsBuilder = new DbContextOptionsBuilder<ExerciseContext>();
optionsBuilder.UseSqlServer(connectionString, options =>
{
    options.EnableRetryOnFailure();
});

using var context = new ExerciseContext(optionsBuilder.Options);

await MainMenu.Show(new ExerciseController(new ExerciseService(new ExerciseRepository(context))));