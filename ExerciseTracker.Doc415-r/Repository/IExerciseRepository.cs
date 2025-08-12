using exerciseTracker.doc415.Models;

namespace exerciseTracker.doc415.Repository;

internal interface IExerciseRepository
{
    IEnumerable<Exercise> GetAll();
    Exercise GetById(int id);
    void Delete(int id);
    void Insert(Exercise exercise);
    void Update(Exercise exercise);
}
