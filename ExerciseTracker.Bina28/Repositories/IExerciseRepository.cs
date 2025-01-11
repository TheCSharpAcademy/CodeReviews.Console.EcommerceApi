using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories;

internal interface IExerciseRepository<T> where T : class
{
	IEnumerable<T> GetAll();
	T GetById(int id);
	void Add(T entity);
	void Update(T entity);
	void Delete(T entity);
}
