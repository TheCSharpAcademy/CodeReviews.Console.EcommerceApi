using Spectre.Console;

namespace ExerciseTracker.K_MYR;

public class Helpers
{
    public static void PrintAndWait(string text)
    {
        AnsiConsole.Write(new Panel($"[springgreen2_1]{text}[/]").BorderColor(Color.DarkOrange3_1));
        Console.ReadKey();
    }
}
