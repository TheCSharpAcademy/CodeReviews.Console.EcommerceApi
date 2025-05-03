using Spectre.Console;

public static class DisplayData
{
    public static void DisplayExercise(List<Exercise> exercises)
    {
        Table table = new();
        table.AddColumns(["Id", "Start Date", "End Date", "Duration", "Additional Comments"]);

        foreach(Exercise exercise in exercises)
            table.AddRow([
                exercise.Id.ToString(),
                exercise.Start.ToString(),
                exercise.End.ToString(),
                exercise.Duration.ToString(),
                exercise.Comments.ToString(),
            ]);

        AnsiConsole.Write(table);
    }
}