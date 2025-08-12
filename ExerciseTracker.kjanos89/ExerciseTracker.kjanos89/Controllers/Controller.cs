using ExerciseTracker.kjanos89.Models;
using ExerciseTracker.kjanos89.Services;

namespace ExerciseTracker.kjanos89.Controllers;

public class Controller
{
    private readonly Service service;
    public Input input;

    public Controller(Service _service)
    {
        service = _service;
        input = new Input();
    }
    public void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("Choose an option from below:");
        Console.WriteLine("1 - List every record");
        Console.WriteLine("2 - Add record");
        Console.WriteLine("3 - Get a record by id");
        Console.WriteLine("4 - Update a record by id");
        Console.WriteLine("5 - Delete a record by id");
        Console.WriteLine("0 - Exit");
        string choice = Console.ReadLine();
        while (String.IsNullOrEmpty(choice))
        {
            Console.WriteLine("Wrong input, please try again!");
            choice = Console.ReadLine();
        }
        Selection(choice[0]);
    }
    public void Selection(char choice)
    {
        switch (choice)
        {
            case '0':
                Environment.Exit(0);
                break;
            case '1':
                GetAll();
                break;
            case '2':
                Add();
                break;
            case '3':
                Read();
                break;
            case '4':
                Update();
                break;
            case '5':
                Delete();
                break;
            default:
                Console.WriteLine("Wrong input, try again!");
                Task.Delay(1000).Wait();
                ShowMenu();
                break;
        }
    }

    private void Delete()
    {
        ListAll();
        int id;
        while (true)
        {
            id = input.GetId();
            if (id == 0)
            {
                ShowMenu();
                return;
            }
            else if (service.IdExists(id))
            {
                break;
            }
        }
        Console.WriteLine("Are you sure you want to delete the record? Answer with 'Y' (yes), 'N' (no) or press '0' to return to the menu.");
        string answer = Console.ReadLine().ToLower();
        if (answer != null && answer == "y")
        {
            service.DeleteExercise(id);
        }
        else if(answer=="0")
        {
            ShowMenu();
            return;
        }
        else
        {
            Console.WriteLine("Removing the record is canceled. Returning to the menu.");
            Thread.Sleep(1000);
            ShowMenu();
            return;
        }
        Console.WriteLine("Record deleted successfully. Returning to the menu.");
        Thread.Sleep(1000);
        ShowMenu();
    }

    private void Update()
    {
        ListAll();
        int id;
        while (true)
        {
            id = input.GetId();
            if (id == 0)
            {
                ShowMenu();
                return;
            }
            else if (service.IdExists(id))
            {
                break;
            }
        }
        var exercise = input.GetExerciseData();
        if (exercise != null)
        {
            service.UpdateExercise(id, exercise.Start, exercise.End, exercise.Duration, exercise.Comments);
        }
        else
        {
            Console.WriteLine("Returned input not acceptable, returning to the menu.");
            Thread.Sleep(1000);
            ShowMenu();
            return;
        }
        Console.WriteLine("Record updated. Returning to the menu.");
        Thread.Sleep(1000);
        ShowMenu();
    }

    private void Read()
    {
        ListAll();
        int id;
        while (true)
        {
            id = input.GetId();
            if(id==0)
            {
                Console.WriteLine("Returning to menu.");
                Thread.Sleep(1000);
                ShowMenu();
                return;
            }
            else if (service.IdExists(id))
            {
                break;
            }
            else
            {
                Console.WriteLine("Id does not exist, try again or press '0' to return to the menu.");
            }
        }
        var exercise = service.ReadExercise(id);
        Console.WriteLine($"Id: {exercise.Id}, Start date: {exercise.Start}, End date: {exercise.End}, Duration: {exercise.Duration}, Comment: {exercise.Comments}");
        Console.WriteLine("\nPressing any button will return to the menu.");
        Console.ReadLine();
        ShowMenu();
    }

    public void GetAll()
    {
        Console.Clear();
        IEnumerable<Exercise> list = service.ListAll();
        foreach (Exercise exercise in list)
        {
            Console.WriteLine($"Id: {exercise.Id}, Start date: {exercise.Start}, End date: {exercise.End}, Whole duration: {exercise.Duration}, Comment: {exercise.Comments}.");
        }
        Console.WriteLine("\nPressing any button will return to the menu.");
        Console.ReadKey();
        ShowMenu();
        return;
    }

    public void ListAll()
    {
        Console.Clear();
        IEnumerable<Exercise> list = service.ListAll();
        foreach (Exercise exercise in list)
        {
            Console.WriteLine($"Id: {exercise.Id}, Start date: {exercise.Start}, End date: {exercise.End}, Whole duration: {exercise.Duration}, Comment: {exercise.Comments}.");
        }
    }

    public void Add()
    {
        Console.Clear();
        var exercise = input.GetExerciseData();
        if (exercise != null)
        {
            service.AddExercise(exercise.Start, exercise.End, exercise.Duration, exercise.Comments);
        }
        else
        {
            Console.WriteLine("Returned input not acceptable, returning to the menu.");
            ShowMenu();
            return;
        }
        ShowMenu();
        return;
    }
}