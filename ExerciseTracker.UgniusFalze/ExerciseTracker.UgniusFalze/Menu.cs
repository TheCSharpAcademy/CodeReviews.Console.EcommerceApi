using ExerciseTracker.UgniusFalze.Controllers;
using ExerciseTracker.UgniusFalze.Models;
using ExerciseTracker.UgniusFalze.Utils;
using Spectre.Console;

namespace ExerciseTracker.UgniusFalze;

public class Menu(ExerciseController exerciseController)
{
    public void Start()
    {
        var exit = false;
        do
        {
            AnsiConsole.Clear();
            var choice = UserInput.DisplayInitialMenu();
            switch (choice)
            {
                case InitialMenuOptions.ViewExercises:
                    var pullups = exerciseController.GetExercises();
                    Display.DisplayExercises(pullups);
                    break;
                case InitialMenuOptions.AddExercise:
                    AddExercise();
                    break;
                case InitialMenuOptions.ManageExercises:
                    ManageExercises();
                    break;
                case InitialMenuOptions.Exit:
                    exit = true;
                    break;
            }
        } while (!exit);
    }

    private void AddExercise()
    {
        var startDate = UserInput.GetStartDate();
        var endDate = UserInput.GetEndDate(startDate);
        var comment = UserInput.GetCommentInput();
        var reps = UserInput.GetRepetitionInput();
        var exercise = new Pullup
            { Comment = comment, DateEnd = endDate, DateStart = startDate, PullupId = 0, Repetitions = reps };
        if (exerciseController.AddExercise(exercise) == false)
        {
            Display.FailedToInsert();
        }

        Display.InsertSucceeded();
        Display.Continue();
    }

    private void ManageExercises()
    {
        var exercise = UserInput.GetExerciseToManage(exerciseController.GetExercises());
        if (exercise == null)
        {
            return;
        }

        var choice = UserInput.DisplayManageMenu();
        switch (choice)
        {
            case ManageMenuOptions.UpdateComment:
                var comment = UserInput.GetCommentInput();
                exercise.Comment = comment;
                break;
            case ManageMenuOptions.UpdateRepetitions:
                var repetitions = UserInput.GetRepetitionInput();
                exercise.Repetitions = repetitions;
                break;
            case ManageMenuOptions.UpdateStartDate:
                var startDate = UserInput.GetStartDate(exercise.DateEnd);
                exercise.DateStart = startDate;
                break;
            case ManageMenuOptions.UpdateEndDate:
                var endDate = UserInput.GetEndDate(exercise.DateStart);
                exercise.DateEnd = endDate;
                break;
            case ManageMenuOptions.Delete:
                if (exerciseController.DeleteExercise(exercise.PullupId) == false)
                {
                    Display.FailedToDelete();
                }
                Display.Continue();
                return;
            case ManageMenuOptions.Exit:
                return;
        }

        if (exerciseController.UpdateExercise(exercise) == false)
        {
            Display.FailedToUpdate();
        }
        Display.UpdateSucceeded();
        Display.Continue();
    }
}