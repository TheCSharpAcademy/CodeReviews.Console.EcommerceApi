
using ExerciseTracker.Interfaces;
using ExerciseTracker.Models;

namespace ExerciseTracker;

internal class ExerciseController
{
    private readonly IExerciseService _service;
    private readonly IExerciseRepository _repository;

    public ExerciseController(IExerciseService service, IExerciseRepository repository)
    {
        _service = service;
        _repository = repository;
    }

    internal void AddExerciseSession()
    {
        var exercise = this._service.CreateNewExercise();
        this._repository.PostExercise(exercise);
    }

    internal void ViewAllExercises()
    {
        this._service.ViewAllExercises();
    }

    internal void UpdateExercise()
    {
        var exercise = this._service.GetExercise();
        if (exercise == null) return;
        this._service.UpdateExercise(exercise);
    }

    internal void DeleteExercise()
    {
        var exercise = this._service.GetExercise();
        if (exercise == null) return;
        this._service.DeleteExercise(exercise);
    }
}
