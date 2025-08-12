using sadklouds.ExcerciseTracker.Services;
using sadklouds.ExcerciseTracker.Visualisation;

namespace sadklouds.ExcerciseTracker.Controllers;

public class ExerciseController : IExerciseController
{
    private readonly IExerciseService _exerciseService;
    public ExerciseController(IExerciseService _exerciseService)
    {
        this._exerciseService = _exerciseService;
    }

    public void Run()
    {
        while (true)
        {
            ExerciseMenu.Menu();
            string option = Console.ReadLine();
            switch(option.ToLower())
            {
                case "v":
                    _exerciseService.GetAllExercises();
                    break;

                case "a":
                    _exerciseService.AddExercise();
                    break;
                case "i":
                    _exerciseService.GetExercise();
                    break;
                case "d":
                    _exerciseService.DeleteExercise();
                    break;
                case "u":
                    _exerciseService.UpdateExercise();
                    break;
            }
        }
    }
}
