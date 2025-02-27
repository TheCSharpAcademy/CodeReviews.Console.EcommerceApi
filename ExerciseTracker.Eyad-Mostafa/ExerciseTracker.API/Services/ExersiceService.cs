using ExerciseTracker.Core.DTOs;
using ExerciseTracker.Core.Models;
using ExerciseTracker.Core.Repositories;

namespace ExerciseTracker.API.Services;

public class ExerciseService : IExerciseService
{
    private readonly IBaseRepository<Exercise> _exerciseRepository;

    public ExerciseService(IBaseRepository<Exercise> exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public Exercise AddExercise(ExerciseDTO exerciseDTO)
    {
        var exercise = new Exercise
        {
            StartDate = exerciseDTO.StartDate.ToUniversalTime(),
            EndDate = exerciseDTO.EndDate.ToUniversalTime(),
            Comments = exerciseDTO.Comments
        };

        ValidateExercise(exercise);

        return _exerciseRepository.Add(exercise);
    }

    public Exercise? UpdateExercise(int id, ExerciseDTO exerciseDTO)
    {
        var exercise = new Exercise
        {
            StartDate = exerciseDTO.StartDate.ToUniversalTime(),
            EndDate = exerciseDTO.EndDate.ToUniversalTime(),
            Comments = exerciseDTO.Comments
        };

        var existingExercise = _exerciseRepository.GetById(id);
        if (existingExercise == null)
        {
            throw new KeyNotFoundException($"Exercise with ID {id} not found.");
        }

        ValidateExercise(exercise);

        return _exerciseRepository.Update(id, exercise);
    }

    public Exercise? DeleteExercise(int id)
    {
        var existingExercise = _exerciseRepository.GetById(id);
        if (existingExercise == null)
        {
            throw new KeyNotFoundException($"Exercise with ID {id} not found.");
        }

        return _exerciseRepository.Delete(id);
    }

    public IEnumerable<Exercise> GetAllExercises()
    {
        return _exerciseRepository.GetAll();
    }

    public Exercise? GetExerciseById(int id)
    {
        return _exerciseRepository.GetById(id);
    }

    private void ValidateExercise(Exercise exercise)
    {
        if (exercise.EndDate < exercise.StartDate)
        {
            throw new ArgumentException("End date cannot be before start date.");
        }

        if (exercise.Comments != null && exercise.Comments.Length > 500)
        {
            throw new ArgumentException("Comments cannot exceed 400 characters.");
        }
    }
}
