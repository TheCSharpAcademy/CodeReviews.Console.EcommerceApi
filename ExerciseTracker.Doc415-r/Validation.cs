namespace exerciseTracker.doc415;
using Spectre.Console;

internal class Validation
{
    static public DateTime GetDate(string input)
    {
        DateTime result = DateTime.Now;
        bool valid = false;
        do
        {
            string startTime = AnsiConsole.Prompt(new TextPrompt<string>($"Enter {input} time of exercise (dd-mm-yyyy hh:mm): "));
            try
            {
                string[] dateTimeComponents = startTime.Split(" ");
                string[] dateComponents = dateTimeComponents[0].Split("-");
                string[] timeComponents = dateTimeComponents[1].Split(":");
                valid = int.TryParse(dateComponents[0], out int day);
                if (!valid) continue;
                valid = int.TryParse(dateComponents[1], out int month);
                if (!valid) continue;
                valid = int.TryParse(dateComponents[2], out int year);
                if (!valid) continue;
                valid = int.TryParse(timeComponents[0], out int hour);
                if (!valid) continue;
                valid = int.TryParse(timeComponents[1], out int minute);
                if (!valid) continue;
                result = new DateTime(year, month, day, hour, minute, 0);
                if (DateTime.TryParse(result.ToString(), out result))
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Please enter a valid date-time.");
                    valid = false;
                }
            }
            catch
            {
                valid = false;
                Console.WriteLine("Please enter a valid date-time.");
            }

        } while (!valid);
        return result;
    }
}
