using ExerciseTrackerCLI.Interface;
using ExerciseTrackerCLI.Models;
using ExerciseTrackerCLI.Services;

namespace ExerciseTrackerCLI.Controllers;

public class ExerciseController(IExerciseService service, Notifications notifications)
{
    
    private void MainMenuHeader()
    {
        Console.Clear();
        UserInput.WelcomeMessage();

        while (notifications.HasNext())
        {
            UserInput.PrintLine(notifications.GetNext());
        }

        UserInput.PrintLine();
    }
    
    public void Run()
    {
        var choice = "";
        while (choice is not "Exit")
        {
            MainMenuHeader();
            choice = UserInput.MainMenuChoice();
            
            // Rewrite the menu to clear notifications.
            MainMenuHeader();
            switch (choice)
            {
                case "Add a run":
                    AddRun();
                    break;
                case "Review runs":
                    ReviewRuns();
                    break;
                case "Exit":
                    break;
            }
        }

        UserInput.Goodbye();
    }

    private void AddRun()
    {
        var run = new TreadmillRun();
        run.DateStart = UserInput.GetStartTime();
        run.DateEnd = UserInput.GetEndTime(run.DateStart);
        run.Comments = UserInput.GetComments();

        service.Add(run);
    }

    private void ReviewRuns()
    {
        if (!service.TryGetAll(out var runs)) return;

        if (runs.Count == 0)
        {
            notifications.AddError("No runs to review.");
            return;
        }
        
        var run = UserInput.RunListMenu(runs);
        ManageRun(run);
    }

    private void ManageRun(TreadmillRun run)
    {
        var selection = UserInput.ManageRunMenu(run);
        switch (selection)
        {
            case "Edit Start Time":
                EditStartTime(run);
                break;
            case "Edit End Time":
                EditEndTime(run);
                break;
            case "Edit Comment":
                EditComment(run);
                break;
            case "Delete Run":
                DeleteRun(run);
                break;
            case "Go Back":
                break;
        }
    }

    private void EditStartTime(TreadmillRun run)
    {
        run.DateStart = UserInput.GetStartTime(run.DateStart);
        service.Update(run);
    }

    private void EditEndTime(TreadmillRun run)
    {
        run.DateEnd = UserInput.GetEndTime(run.DateEnd, run.DateStart);
        service.Update(run);
    }

    private void EditComment(TreadmillRun run)
    {
        run.Comments = UserInput.GetComments(run.Comments);
        service.Update(run);
    }

    private void DeleteRun(TreadmillRun run)
    {
        service.Delete(run);
    }
}