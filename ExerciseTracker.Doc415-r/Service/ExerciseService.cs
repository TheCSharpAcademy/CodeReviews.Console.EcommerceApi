using exerciseTracker.doc415.Models;
using exerciseTracker.doc415.Repository;

namespace exerciseTracker.doc415.Service;

internal class ExerciseService
{
    IExerciseRepository _repository;
    IExerciseRepository _repositoryDapper;

    public ExerciseService()
    {
        _repository = new ExerciseRepository();
        _repositoryDapper = new ExerciseRepositoryDapper();
    }

    public void AddExercise(string type, DateTime dateStart, DateTime dateEnd, TimeSpan duration, string comments)
    {
        Exercise newExercise = new Exercise
        {
            DateStart = dateStart,
            DateEnd = dateEnd,
            Duration = duration,
            Comments = comments,
            Type = type
        };
        if (type == "Cardio")
            _repository.Insert(newExercise);
        else
            _repositoryDapper.Insert(newExercise);

    }

    public IEnumerable<Exercise> GetExerciseList()
    {
        return _repository.GetAll();
    }

    public void DeleteExercise(int id)
    {
        _repository.Delete(id);
    }

    public void UpdateExercise(int id, string type, DateTime dateStart, DateTime dateEnd, TimeSpan duration, string comments)
    {
        Exercise newExercise = new Exercise
        {
            Id = id,
            DateStart = dateStart,
            DateEnd = dateEnd,
            Duration = duration,
            Comments = comments,
            Type = type
        };
        if (type == "Cardio")
            _repository.Update(newExercise);
        else
            _repositoryDapper.Update(newExercise);

    }

    public Exercise GetExerciseById(int id)
    {
        return _repository.GetById(id);
    }
}
