using ConsoleTableExt;
using GymExerciseTracker.Controllers;
using GymExerciseTracker.Dtos;
using GymExerciseTracker.Models;

namespace GymExerciseTracker.Utils;
public class UserInput
{
    private readonly ExerciseController _exerciseController;
    bool keepGoing = true;
    public UserInput(ExerciseController exerciseController)
    {
        _exerciseController = exerciseController;
    }

    public void Start()
    {
        while (keepGoing)
        {
            Console.WriteLine("\nMain Menu");
            Console.WriteLine("---------\n");
            Console.WriteLine("What would you like to do?\n");
            Console.WriteLine("Type 1 to View all Gym sessions");
            Console.WriteLine("Type 2 to Insert Gym session");
            Console.WriteLine("Type 3 to Delete a Gym session");
            Console.WriteLine("Type 4 to Update a Gym session\n");
            Console.WriteLine("Type 0 to Close Application");
            Console.WriteLine("---------------------------\n");

            Console.Write("Enter a number: ");
            string? command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    CloseApp();
                    break;
                case "1":
                    GetAllGymSessions();
                    break;
                case "2":
                    InsertGymSession();
                    break;
                case "3":
                    DeleteGymSession();
                    break;
                case "4":
                    UpdateGymSession();
                    break;
                default:
                    Console.WriteLine("Invalid command.Try again.");
                    break;
            }
        }
    }
    void GetAllGymSessions()
    {
        Console.Clear();
        var gymSessions = _exerciseController.GetAllGymSessions();
        var tableData = new List<List<object>>();

        foreach (var gymSession in gymSessions)
        {
            tableData.Add(new List<object> { gymSession.Id, gymSession.Name, gymSession.Sets, gymSession.Reps, gymSession.StartDate, gymSession.EndDate, gymSession.Duration, gymSession.Comments });
        }

        ConsoleTableBuilder
        .From(tableData)
        .WithColumn("Id", "Name", "Sets", "Reps", "Start Date", "End Date", "Duration", "Comments")
        .ExportAndWriteLine();
    }
    void InsertGymSession()
    {
        string? name = GetString("Enter a name: ");

        int sets = GetNumber("Enter number of sets(i.e 1, 3, 5): ");

        int reps = GetNumber("Enter number of reps(i.e 1, 3, 5): ");

        string startDate = GetDate("Enter a start date(format: dd/mm/yyyy HH:MM i.e 20/10/2023 14:54): ");

        string endDate = GetDate("Enter a end date(format: dd/mm/yyyy HH:MM i.e 20/10/2023 14:54): ");

        while (!Validate.IsValidDateRange(DateTime.Parse(startDate), DateTime.Parse(endDate)))
        {
            Console.WriteLine("End date can't be earlier than start date.Try again.");
            endDate = GetDate("Enter a end date(format: dd/mm/yyyy HH:MM i.e 20/10/2023 14:54): ");
        }

        string comments = GetString("Enter comments: ");

        AddGymSessionDto addGymSessionDto = new AddGymSessionDto
        {
            Name = name,
            Sets = sets,
            Reps = reps,
            StartDate = DateTime.Parse(startDate),
            EndDate = DateTime.Parse(endDate),
            Comments = comments
        };

        GymSession gymSession = _exerciseController.AddGymSession(addGymSessionDto);

        if (gymSession == null)
        {
            Console.WriteLine("Failed to insert record.");
        }
        else
        {
            Console.WriteLine("The record has been added!");
        }
    }
    void DeleteGymSession()
    {
        GetAllGymSessions();
        var sessionId = GetNumber("Enter a session id: ");

        var deletedSession = _exerciseController.DeleteGymSession(sessionId);

        while (deletedSession == null)
        {
            Console.WriteLine("No record found.Try again.");
            sessionId = GetNumber("Enter a session id: ");
            deletedSession = _exerciseController.GetGymSession(sessionId);
        }

        Console.WriteLine("Record deleted!");
    }
    void UpdateGymSession()
    {
        GetAllGymSessions();

        int sessionId = GetNumber("Enter a id: ");

        var session = _exerciseController.GetGymSession(sessionId);

        while (session == null)
        {
            Console.WriteLine("No records found.Try again.");

            sessionId = GetNumber("Enter a id: ");
            session = _exerciseController.GetGymSession(sessionId);
        }

        string? name = GetString("Enter a name: ");

        int sets = GetNumber("Enter number of sets(i.e 1, 3, 5): ");

        int reps = GetNumber("Enter number of reps(i.e 1, 3, 5): ");

        string startDate = GetDate("Enter a start date(format: dd/mm/yyyy HH:MM i.e 20/10/2023 14:54): ");

        string endDate = GetDate("Enter a end date(format: dd/mm/yyyy HH:MM i.e 20/10/2023 14:54): ");

        while (!Validate.IsValidDateRange(DateTime.Parse(startDate), DateTime.Parse(endDate)))
        {
            Console.WriteLine("End date can't be earlier than start date.Try again.");
            endDate = GetDate("Enter a end date(format: dd/mm/yyyy HH:MM i.e 20/10/2023 14:54): ");
        }

        string comments = GetString("Enter comments: ");

        UpdateGymSessionDto updatedGymSessionDto = new UpdateGymSessionDto
        {
            Id = sessionId,
            Name = name,
            Sets = sets,
            Reps = reps,
            StartDate = DateTime.Parse(startDate),
            EndDate = DateTime.Parse(endDate),
            Comments = comments
        };

        var result = _exerciseController.UpdateGymSession(sessionId, updatedGymSessionDto);

        if (result == null)
        {
            Console.WriteLine("Fail to update record.");
        }
        else
        {
            Console.WriteLine("Record has been updated!");
        }
    }
    string? GetDate(string message)
    {
        Console.Write(message);
        string? date = Console.ReadLine()?.Trim();

        while (!Validate.IsValidateDate(date))
        {
            Console.WriteLine("Invalid date.Try again.");
            Console.Write(message);

            date = Console.ReadLine();
        }
        return date;
    }
    string? GetString(string message)
    {
        Console.Write(message);
        string input = Console.ReadLine()?.Trim();

        while (!Validate.IsValidString(input))
        {
            Console.WriteLine("Invalid input.Try again.");
            Console.Write(message);
            input = Console.ReadLine()?.Trim();
        }

        return input;
    }
    int GetNumber(string message)
    {
        Console.Write(message);
        string inputNumber = Console.ReadLine()?.Trim();

        while (!Validate.IsValidNumber(inputNumber))
        {
            Console.WriteLine("Invalid number.Try again.");
            Console.Write(message);
            inputNumber = Console.ReadLine()?.Trim();
        }

        int number = int.Parse(inputNumber);

        return number;
    }
    void CloseApp()
    {
        keepGoing = false;
    }
}
