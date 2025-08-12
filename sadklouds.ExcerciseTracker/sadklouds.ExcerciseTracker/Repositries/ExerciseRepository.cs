using sadklouds.ExcerciseTracker.DBContext;
using sadklouds.ExcerciseTracker.Models;

namespace sadklouds.ExcerciseTracker.Repositries;

public class ExerciseRepository : IExerciseRepository
{

    private readonly ExerciseContext _context;

    public ExerciseRepository(ExerciseContext _context)
    {
        this._context = _context;
    }

    public void Add(ExerciseModel entity)
    {
         _context.Add(entity);
        _context.SaveChanges();
    }

    public IEnumerable<ExerciseModel> GetAll()
    {
        return _context.Set<ExerciseModel>().ToList();
    }

    public ExerciseModel GetById(int id)
    {
        return _context.Find<ExerciseModel>(id);
    }

    public void Delete(ExerciseModel exercise)
    {
        _context.Remove(exercise);
        _context.SaveChanges();
    }

    public void Update(ExerciseModel updatedEntity, ExerciseModel curentEntity)
    {
        curentEntity.StartDate = updatedEntity.StartDate;
        curentEntity.EndDate = updatedEntity.EndDate;
        curentEntity.Duration = updatedEntity.Duration;
        curentEntity.Comments = updatedEntity.Comments;
        
        _context.Update(curentEntity);
        _context.SaveChanges();
    }
}
