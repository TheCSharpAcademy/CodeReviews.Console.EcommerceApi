using ExerciseTracker.kwm0304.Models;
using ExerciseTracker.kwm0304.Services;
using ExerciseTracker.kwm0304.Views;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExerciseTracker.kwm0304.Controllers
{
  public class ExerciseController
  {
    private readonly ExerciseService _service;
    public ExerciseController(ExerciseService service)
    {
      _service = service;
    }

    internal async Task<bool> HandleAddEntry()
    {
      string title = StringPrompt.GetAndConfirmResponse<string>("What exercise are you logging?");
      string comments = StringPrompt.GetAndConfirmResponse<string>("Would you like to add any comments?");
      DateTime startingDate = StringPrompt.ReturnDateTime("start");
      DateTime endingDate = StringPrompt.ReturnDateTime("end");
      bool validDates = ValidateStartAndEnd(startingDate, endingDate);
      if (validDates)
      {
        TimeSpan duration = endingDate - startingDate;
        UserInput input = new()
        {
          StartDate = startingDate,
          EndDate = endingDate,
          Duration = duration,
          Title = title,
          Comments = comments
        };
        await _service.CreateExerciseAsync(input);
        AnsiConsole.WriteLine("Entry successfully added");
        return true;
      }
      return false;
    }

    private static bool ValidateStartAndEnd(DateTime start, DateTime end)
    {
      return end > start;
    }

    internal async Task<bool> HandleViewEntries()
    {
      List<UserInput> entries = await _service.GetAllExercisesAsync();
      if (entries.Count < 1)
      {
        AnsiConsole.WriteLine("No entries to display yet");
        Thread.Sleep(1500);
        return true;
      }
      UserInput entry = SelectionMenus.UserInputSelection(entries);
      AnsiConsole.WriteLine(entry.ToString());
      string choice = SelectionMenus.EntriesMenu();
      bool running = true;
      while (running)
      {
        switch (choice)
        {
          case "Edit":
            await HandleEdit(entry);
            return true;
          case "Delete":
            await HandleDelete(entry);
            return true;
          case "Back":
            running = false;
            return true;
          default:
            running = true;
            return true;
        }
      }
      return true;
    }

    private async Task HandleDelete(UserInput entry)
    {
      AnsiConsole.WriteLine(entry.ToString());
      bool confirmDelete = AnsiConsole.Confirm("Are you sure you want to delete this entry?");
      if (confirmDelete)
      {
        await _service.DeleteExerciseByIdAsync(entry.Id);
        AnsiConsole.WriteLine("Entry successfully deleted");
      }
    }

    private async Task HandleEdit(UserInput entry)
    {
      string choice = SelectionMenus.EditChoice();
      switch (choice)
      {
        case "Title":
          string title = StringPrompt.GetAndConfirmResponse<string>("What would you like to change the title to?");
          entry.Title = title;
          break;
        case "Start Date":
          DateTime newStart = StringPrompt.ReturnDateTime("start");
          entry.StartDate = newStart;
          break;
        case "End Date":
          DateTime newEnd = StringPrompt.ReturnDateTime("end");
          entry.EndDate = newEnd;
          break;
        case "Comments":
          string comments = StringPrompt.GetAndConfirmResponse<string>("What do you want the comments to say?");
          entry.Comments = comments;
          break;
        default:
          break;
      }
      bool isValid = ValidateStartAndEnd(entry.StartDate, entry.EndDate);
      if (isValid)
      {
        entry.Duration = entry.EndDate - entry.StartDate;
        await _service.UpdateExerciseAsync(entry);
        AnsiConsole.WriteLine("Entry updated successfully");
        return;
      }
      else
      {
        AnsiConsole.WriteLine("End date cannot be before start date");
        return;
      }
    }
  }
}