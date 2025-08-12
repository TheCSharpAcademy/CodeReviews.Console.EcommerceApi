using Spectre.Console;
using STUDY.ConsoleProjects.ExerciseTrackerFour.Data.Repository;
using STUDY.ConsoleProjects.ExerciseTrackerFour.Models;
using STUDY.ConsoleProjects.ExerciseTrackerFour.Service;

namespace STUDY.ConsoleProjects.ExerciseTrackerFour.Controller;
public class ExerciseController
{
    private readonly IExerciseService _exerciseService;
    private readonly IExerciseRepository _exerciseRepository;
    public ExerciseController(IExerciseService exerciseService, IExerciseRepository exerciseRepository)
    {
        _exerciseService = exerciseService;
        _exerciseRepository = exerciseRepository;
    }
    public void ViewAllExerciseEntries()
    {
        var exercises = _exerciseRepository.GetExercises();

        if (exercises.Count == 0)
        {
            AnsiConsole.MarkupLine("404 - Cannot be found");
            MainMenu.ShowMainMenu();
        }

        else
        {
            DataVisualization.ShowDataInTable(exercises);
        }

        Console.WriteLine("View All Exercise Entries completed");
    }
    public void ViewSpecificExerciseEntry()
    {
        Console.WriteLine("Enter the ID of the exercise entry you want to view:");
        int exerciseId = int.Parse(Console.ReadLine());

        Exercise? selectedExercise = _exerciseRepository.GetExerciseEntryById(exerciseId);

        if (selectedExercise is null)
        {
            AnsiConsole.MarkupLine("404 - Cannot be found");
            MainMenu.ShowMainMenu();
        }

        Exercise exercise = _exerciseRepository.ViewSpecificExerciseEntry(exerciseId);

        DataVisualization.ShowSingleExercise(exercise);
    }
    public void AddExerciseEntry()
    {
        _exerciseService.AddExerciseEntry();
    }
    public void UpdateExerciseEntry()
    {
        _exerciseService.UpdateExerciseEntry();
    }
    public void DeleteExerciseEntry()
    {
        _exerciseService.DeleteExerciseEntry();
    }
}
