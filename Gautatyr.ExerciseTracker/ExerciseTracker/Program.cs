using ExerciseTracker;
using ExerciseTracker.Controllers;
using ExerciseTracker.Models;
using ExerciseTracker.Repositories;

ExerciseTrackerContext context = new ExerciseTrackerContext();
RunRepository runRepository = new RunRepository(context);
RunController runController = new(runRepository);

//For testing purposes
Run newRun = new()
{
    Start = DateTime.Now,
    End = DateTime.Now,
    Distance = "15",
    Comment = "This is a test"
};

newRun.SetDuration();

await runController.CreateRunAsync(newRun);

UserInput userInput = new(runController);

await userInput.MainMenuAsync();