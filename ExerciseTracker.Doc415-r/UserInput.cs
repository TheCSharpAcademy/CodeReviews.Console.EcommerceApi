using Spectre.Console;

namespace exerciseTracker.doc415;

internal class UserInput
{
    static public string GetModelType()
    {
        string type = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Select exercise type")
                                                                      .AddChoices("Cardio",
                                                                                  "Weight Lifting"));
        return type;
    }

    static public (DateTime, DateTime) GetDates()
    {
        DateTime startDate = new();
        DateTime endDate = new();
        bool valid = false;
        do
        {
            startDate = Validation.GetDate("start");
            endDate = Validation.GetDate("end");
            var difference = (endDate - startDate).TotalMinutes;
            if (difference >= 0) valid = true;
            else
                Console.WriteLine("Exercise duration can't be negative or zero. Check your date-times.");
        } while (!valid);
        return (startDate, endDate);
    }

    static public string GetComments()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("Enter comments: "));

    }

    static public int GetId(string title)
    {
        return AnsiConsole.Prompt(new TextPrompt<int>($"Enter record id to {title}: ").
                                                       ValidationErrorMessage("Enter a valid number.").
                                                       Validate(_id =>
                                                       {
                                                           return _id switch
                                                           {
                                                               < 0 => ValidationResult.Error("Id can't be negative number"),
                                                               _ => ValidationResult.Success()
                                                           };
                                                       }));
    }

}



