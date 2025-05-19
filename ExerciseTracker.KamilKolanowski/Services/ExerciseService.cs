using ExerciseTracker.KamilKolanowski.Models;
using ExerciseTracker.KamilKolanowski.Repositories;
using Spectre.Console;

namespace ExerciseTracker.KamilKolanowski.Services;

public class ExerciseService
{
    private readonly UserInputService _userInputService;
    private readonly IExerciseRepository _repository;
    private readonly ExerciseService _service;

    public ExerciseService(IExerciseRepository repository, UserInputService userInputService)
    {
        _repository = repository;
        _userInputService = userInputService;
    }
    
    internal void AddExercise()
    {
        var exercise = CreateExercise();

        _repository.Insert(exercise);
    }

    internal void UpdateExercise()
    {
        var exerciseId = PromptForId("update");
        var exercise = _repository.GetExercise(exerciseId);
        
        if (exercise == null) return;

        var updatedExercise = _userInputService.EditExercise(exercise);

        _repository.Update(updatedExercise);
    }

    internal void DeleteExercise()
    {
        var exerciseId = PromptForId("delete");
        var exercise = _repository.GetExercise(exerciseId);

        if (exercise == null) return;

        _repository.Delete(exerciseId);
    }

    internal IEnumerable<Exercise?> ReadExercises()
    {
        return _repository.GetExercises();
    }

    private Exercise CreateExercise()
    {
        return _userInputService.CreateExercise();
    }
    
    private int PromptForId(string operation)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>($"Select exercise id to {operation}:")
                .Validate(id => _repository.GetExercise(id) != null));
    }
}
