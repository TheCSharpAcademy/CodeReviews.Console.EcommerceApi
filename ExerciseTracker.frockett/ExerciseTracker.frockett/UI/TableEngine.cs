using ExerciseTracker.frockett.Models;
using Spectre.Console;

namespace ExerciseTracker.frockett.UI;

public class TableEngine
{
    public void PrintSessions(List<ExerciseSession> sessions)
    {
        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[red]No sessions found![/]");
            return;
        }

        Table table = new Table();
        table.Alignment(Justify.Center);
        table.Title("Exercise Sessions");
        table.AddColumns(new[] { "Start Time", "End Time", "Duration", "Comments" });

        foreach (ExerciseSession session in sessions)
        {
            table.AddRow(session.StartTime.ToString("MMM dd yyyy HH:mm"), session.EndTime.ToString("MMM dd yyyy HH:mm"), session.Duration.ToString(), session.Comments);
        }

        AnsiConsole.Write(table);
    }

    public ExerciseSession SelectSession(List<ExerciseSession> sessions)
    {
        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[red]No sessions found![/]");
            return null;
        }

        var selection = new SelectionPrompt<ExerciseSession>();
        selection.AddChoices(sessions);
        selection.Title("Select session")
        .UseConverter(session => $"{session.Duration.Hours}h{session.Duration.Minutes}m long session on {session.StartTime:yyyy/MM/dd} from {session.StartTime:HH:mm} to {session.EndTime:HH:mm} // Comment: {session.Comments}");

        ExerciseSession selectedSession = AnsiConsole.Prompt(selection);

        return selectedSession;
    }
}
