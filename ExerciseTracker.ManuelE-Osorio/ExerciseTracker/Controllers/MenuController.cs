using ExerciseTracker.Models;
using ExerciseTracker.UserInterface;

namespace ExerciseTracker.Controllers;

public class MenuController(RunningController runningController)
{
    public RunningController RunningControllerInstance = runningController;

    public void Start()
    {
        MainUI.DisplayWelcomeMessage();
        MainMenu();
    }

    public void MainMenu()
    {
        ConsoleKey? pressedKey = null;
        bool runMainMenu = true;
        string? errorMessage = null;
        do
        {
            if(pressedKey != ConsoleKey.D1)
                MainUI.ClearUI();
            MainUI.DisplayMainMenu(errorMessage);
            pressedKey = Console.ReadKey(true).Key;
            switch (pressedKey)
            {
                case(ConsoleKey.D1):
                    errorMessage = ListExerciseMenu();
                    break;
                case(ConsoleKey.D2):
                    if(InsertExerciseMenu())
                        MainUI.DisplaySuccessMessage();
                    errorMessage = null;
                    break;
                case(ConsoleKey.D3):
                    errorMessage = UpdateMenu();
                    break;
                case(ConsoleKey.D4):
                    errorMessage = DeleteMenu();
                    break;      
                case(ConsoleKey.Escape):
                case(ConsoleKey.Backspace):
                    runMainMenu = false;
                    break;                                  
            }
        }
        while(runMainMenu);
        MainUI.DisplayExitMessage();
    }

    public string? ListExerciseMenu()
    {
        var exerciseList = RunningControllerInstance.GetAll();
        if (exerciseList == null || exerciseList.Count == 0)
        {
            MainUI.ClearUI();
            return "The log is empty";
        }
        else 
            MainUI.DisplayExerciseList(exerciseList);
        return null;
    }

    public bool InsertExerciseMenu()
    {
        return RunningControllerInstance.Insert();
    }

    public string? UpdateMenu()
    {
        var exerciseToUpdate = RunningControllerInstance.GetById();
        if(exerciseToUpdate == null)
            return "The log is empty";

        bool runUpdateMenu = true;
        ConsoleKey pressedKey;

        do
        {
            MainUI.DisplayModifyMenu<Running>(exerciseToUpdate);
            pressedKey = Console.ReadKey(true).Key;
            switch(pressedKey)
            {
                case(ConsoleKey.D1):
                case(ConsoleKey.D2):
                case(ConsoleKey.D3):
                    if(RunningControllerInstance.Update((UpdateOptions) (Convert.ToInt32(pressedKey) - 49), 
                        exerciseToUpdate))
                        MainUI.DisplaySuccessMessage();
                    break;
                
                case(ConsoleKey.Backspace):
                case(ConsoleKey.Escape):
                    runUpdateMenu = false;
                    break;
            }
        }
        while(runUpdateMenu);
        return null;
    }

    public string? DeleteMenu()
    {
        var exerciseToDelete = RunningControllerInstance.GetById();
        if(exerciseToDelete == null)
            return "The log is empty";

        if(InputController.GetConfirmation(ConfirmationOptions.delete) == true)
            if(RunningControllerInstance.Delete(exerciseToDelete))
                MainUI.DisplaySuccessMessage();
        return null;
    }
}