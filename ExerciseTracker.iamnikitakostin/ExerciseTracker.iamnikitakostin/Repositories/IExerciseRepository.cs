using ExerciseTracker.iamnikitakostin.Models;

namespace ExerciseTracker.iamnikitakostin.Repositories;
internal interface IExerciseRepository
{
    List<Exercise> GetAll();
    Dictionary<int, string> GetAllAsDictionary();
    Exercise GetById(int id);
    bool Add(Exercise exercise);
    bool Update(Exercise exercise);
    bool Delete(int id);
}
