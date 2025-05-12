using ExerciseTracker.Models;
using ExerciseTracker.Repositories;
using ExerciseTracker.Services;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace ExerciseTracker.Controllers;

class ExerciceService
{
    private readonly ExerciceController<FieldTours>? _fieldToursService;
    private readonly ExerciceController<FreeKicks>? _freeKicksService;
    private readonly ExerciceRepository<FieldTours>? _fieldToursRepository;
    private readonly ExerciceRepository<FreeKicks>? _freeKicksRepository;
    private readonly string _type;

    public ExerciceService(DbContext context, string type)
    {
        _type = type;
        if (_type == "FieldTours")
        {
            _fieldToursRepository = new ExerciceRepository<FieldTours>(context);
            _fieldToursService = new(_fieldToursRepository);
        }
        else
        {
            _freeKicksRepository = new ExerciceRepository<FreeKicks>(context);
            _freeKicksService = new(_freeKicksRepository);
        }
    }

    internal void CreateExercice()
    {
        bool valid = false;
        DateTime start = new();
        DateTime end = new();
        while (!valid)
        {
            AnsiConsole.WriteLine("Start");
            start = UserInputs.GetDateTime();
            AnsiConsole.WriteLine("End");
            end = UserInputs.GetDateTime();
            valid = start < end;
            if (!valid) AnsiConsole.MarkupLine("The start must be [red]before[/] the end.");
        }

        string comment = UserInputs.GetComment(null);
        if (string.IsNullOrWhiteSpace(comment)) comment = "";
        
        if (_type == "FieldTours")
        {
            FieldTours exercice = new(_fieldToursService.GetExercices().Count + 1, start, end, comment);
            _fieldToursService.AddExercice(exercice);
        }
        else
        {
            FreeKicks exercice = new(_freeKicksService.GetExercices().Count + 1, start, end, comment);
            _freeKicksService.AddExercice(exercice);
        }
    }

    internal void DeleteExercice()
    {
        IExercices exercice = GetSingleExercice();
        if (exercice.Id != 0 && UserInputs.Validation("Are you sure you want to delete this exercice?"))
        {
            if (_type == "FieldTours") _fieldToursService.DeleteExercice((FieldTours)exercice);
            else _freeKicksService.DeleteExercice((FreeKicks)exercice);

        }
    }

    internal void UpdateExercice()
    {
        IExercices exercice = GetSingleExercice();
        if (exercice.Id != 0)
        {
            string option = UserInputs.ChooseUpdateOption();
            if (_type == "FieldTours")
            {
                switch (option)
                {
                    case "Start":
                        exercice = UserInputs.UpdateStart(exercice);
                        break;
                    case "End":
                        exercice = UserInputs.UpdateEnd(exercice);
                        break;
                    case "Comment":
                        AnsiConsole.MarkupLine($"[Blue]Current[/] comment:\n{exercice.Comment}\n");
                        exercice.Comment = UserInputs.GetComment(exercice);
                        break;
                    default:
                        break;
                }
                if(UserInputs.Validation("Are you sure you want to update this exercice?")) _fieldToursService.UpdateExercice((FieldTours)exercice);
            }
            else
            {
                switch (option)
                {
                    case "Start":
                        exercice = UserInputs.UpdateStart(exercice);
                        break;
                    case "End":
                        exercice = UserInputs.UpdateEnd(exercice);
                        break;
                    case "Comment":
                        exercice.Comment = UserInputs.GetComment(exercice);
                        break;
                    default:
                        break;
                }
                if(UserInputs.Validation("Are you sure you want to update this exercice?")) _freeKicksService.UpdateExercice((FreeKicks)exercice);
            }
        }
    }

    internal void PrintOneExercice()
    {
        IExercices exercice = GetSingleExercice();
        if (exercice.Id != 0) DataOutput.PrintSingleExercice(exercice);
    }

    internal void PrintExercices()
    {
        IExercices[] exercices;
        if (_type == "FieldTours") exercices = [.. _fieldToursService.GetExercices()];
        else exercices = [.._freeKicksService.GetExercices()];
        if (exercices != null) DataOutput.PrintAllExercices(exercices);
        else AnsiConsole.WriteLine("No Data found in the database.");
    }

    private IExercices GetSingleExercice()
    {
        IExercices exercice;
        if (_type == "FieldTours")
        {
            List<FieldTours> tours = _fieldToursService.GetExercices();
            tours.Add(new FieldTours(0, DateTime.Parse($"{DateTime.Today}"), DateTime.Parse($"{DateTime.Today}"), "Cancel"));
            exercice = UserInputs.ChoooseExercice([.. tours]);
        }
        else
        {
            List<FreeKicks> kicks = _freeKicksService.GetExercices();
            kicks.Add(new FreeKicks(0, DateTime.Parse($"{DateTime.Today}"), DateTime.Parse($"{DateTime.Today}"), "Cancel"));
            exercice = UserInputs.ChoooseExercice([.. kicks]);
        }
        return exercice;
    }
}