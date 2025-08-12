using sadklouds.ExcerciseTracker.Models;

namespace sadklouds.ExcerciseTracker.Repositries;
public interface IExerciseRepository
{
    public void Add(ExerciseModel entity);
    public void Update(ExerciseModel updatedEntity, ExerciseModel curentEntity);
    public ExerciseModel GetById(int id);
    public IEnumerable<ExerciseModel> GetAll();
    public void Delete(ExerciseModel exercise);
}
