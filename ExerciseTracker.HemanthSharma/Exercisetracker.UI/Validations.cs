using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseTracker.UI
{
    internal class Validations
    {
        internal static DateTime GetValidDate()
        {
            AnsiConsole.MarkupLine("[darkred]Enter the Date in 'dd-MM-yyyy' Format:[/]");
            bool res = DateTime.TryParseExact(Console.ReadLine().Trim(), "dd-MM-yyyy", null, DateTimeStyles.None, out DateTime DateResult);
            while (!res)
            {
                AnsiConsole.MarkupLine("[darkred]Enter the Date in correct [yellow] 'dd-MM-yyyy'[/] Format:[/]");
                res = DateTime.TryParseExact(Console.ReadLine().Trim(), "dd-MM-yyyy", null, DateTimeStyles.None, out DateResult);
            }
            return DateResult;
        }

        internal static DateTime  GetValidTime()
        {
            AnsiConsole.MarkupLine("[lightsteelblue]Enter the Time in 'HH:mm' Format:[/]");
            bool res = DateTime.TryParseExact(Console.ReadLine().Trim(), "HH:mm", null, DateTimeStyles.None, out DateTime TimeResult);
            while (!res)
            {
                AnsiConsole.MarkupLine("[red3_1]Enter the Time in correct [yellow] 'HH:mm'[/] Format:[/]");
                res = DateTime.TryParseExact(Console.ReadLine().Trim(), "HH:mm", null, DateTimeStyles.None, out TimeResult);
            }
            return TimeResult;
        }
    }
}
