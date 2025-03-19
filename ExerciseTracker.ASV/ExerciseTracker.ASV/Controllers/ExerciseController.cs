using ExerciseTracker.ASV.Views;
using ExerciseTracker.ASV.Services;

namespace ExerciseTracker.ASV.Controllers;

public class ExerciseController : IExerciseController
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService, IDisplay display) { 
        _exerciseService = exerciseService;
    }

    public async Task Start()
    {
        string selection = _exerciseService.GetSelection();
        while (selection != "Exit Application")
        {
            if(selection == "View Past Workouts")
            {
                await _exerciseService.DisplayAllWorkoutsAsync();
            }
            else if (selection == "Create a new workout")
            {
                await _exerciseService.CreateWorkoutAsync();
            }
            else if(selection == "Edit a previous workout")
            {
                await _exerciseService.EditWorkoutAsync();
            }
            else if (selection == "Delete a workout")
            {
                await _exerciseService.DeleteWorkoutAsync();
            }
            else if (selection == "Exit Application")
            {
                Console.WriteLine("Goodbye!!!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice!");
            }
            Console.Clear();
            selection = _exerciseService.GetSelection();
        }
    }
}