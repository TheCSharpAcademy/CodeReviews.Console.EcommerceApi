namespace App.Util;

public static class UiUtil
{
    public static char PressKeyToContinue(string message = "Press any key to continue")
    {
        Console.WriteLine(message);
        var keyChar = Console.ReadKey().KeyChar;
        Console.Clear();

        return keyChar;
    }

    public static string FormatDuration(TimeSpan duration)
    {
        return string.Format(
            $"{(int)duration.TotalHours,-3} h, {duration.Minutes,-3} m"
        );
    }
}
