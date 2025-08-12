using ExerciseTracker.kwm0304.Models;

namespace ExerciseTracker.kwm0304.Views;

public class SelectionMenus
{
  public const string genericPrompt = "What would you like to do?";
    public static string MainMenu()
    {
      var menuOptions = new List<string>{"Add entry", "View entries", "Exit"};
      var menu = new BasePrompt<string>(genericPrompt, menuOptions);
      return menu.Show()!;
    }

    public static string EntriesMenu()
    {
      var menuOptions = new List<string>{"Edit", "Delete", "Back"};
      var menu = new BasePrompt<string>(genericPrompt, menuOptions);
      return menu.Show()!;
    }

    public static UserInput UserInputSelection(List<UserInput> entries)
    {
      var menu = new BasePrompt<UserInput>("Choose entry: ", entries);
      return menu.Show()!;
    }
    public static string EditChoice()
    {
      var menuOptions = new List<string> {"Title", "Start Date", "End Date", "Comments"};
      var menu = new BasePrompt<string>("What would you like to change?", menuOptions);
      return menu.Show()!;
    }
}