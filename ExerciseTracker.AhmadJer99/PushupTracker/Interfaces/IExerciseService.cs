using ExerciseTracker.Models;

namespace ExerciseTracker.Interfaces;

public interface IExerciseService
{
    public Task<List<Pushup>> GetAll();
    public Task<Pushup> GetById(int id);
    public Task Create(Pushup newEntity);
    public Task Update(Pushup updatedEntity);
    public Task Delete(Pushup entity);
}