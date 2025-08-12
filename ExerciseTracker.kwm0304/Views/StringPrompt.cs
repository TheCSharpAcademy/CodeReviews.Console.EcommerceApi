using Spectre.Console;

namespace ExerciseTracker.kwm0304.Views;

public class StringPrompt
{
  public static T GetAndConfirmResponse<T>(string question)
  {
    while (true)
    {
      T answer = AnsiConsole.Ask<T>(question);

      if (answer != null && !string.IsNullOrWhiteSpace(answer.ToString()))
      {
        bool confirm = AnsiConsole.Confirm("Are you sure?");
        if (confirm)
          return answer;
      }
      else
      {
        AnsiConsole.MarkupLine("[bold red]Response cannot be empty.[/]");
        AnsiConsole.WriteLine("Please enter a valid response:");
      }
    }
  }

  public static DateTime ReturnDateTime(string v)
  {
    DateTime date = GetDate(v);
    TimeSpan time = GetTime(v);
    return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
  }
  private static DateTime GetDate(string v)
  {
    while (true)
    {
      var input = AnsiConsole.Ask<string>($"What date did you {v} (YYYY-MM-DD):");

      if (DateTime.TryParse(input, out DateTime date))
      {
        return date;
      }
      else
      {
        AnsiConsole.MarkupLine("[red]Invalid date format. Please try again.[/]");
      }
    }
  }
  private static TimeSpan GetTime(string v)
  {
    while (true)
    {
      var input = AnsiConsole.Ask<string>($"What time did you {v} (HH:MM:SS):");
      AnsiConsole.WriteLine("Ex: If 11PM -> 23:00:00");
      if (TimeSpan.TryParse(input, out TimeSpan time))
      {
        return time;
      }
      else
      {
        AnsiConsole.MarkupLine("[red]Invalid time format. Please try again.[/]");
      }
    }
  }
}