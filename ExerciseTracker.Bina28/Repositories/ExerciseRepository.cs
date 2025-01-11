
using ExerciseTracker.Data;
using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Repositories;

internal class ExerciseRepository<T> : IExerciseRepository<T> where T : class
{
	private readonly ExerciseContext _context;

	public ExerciseRepository(ExerciseContext context)
	{
		_context = context;
	}

	public void Add(T model)
	{
		_context.Set<T>().Add(model);
		_context.SaveChanges();
	}

	public void Delete(T model)
	{
		_context.Set<T>().Remove(model);
		_context.SaveChanges();
	}

	public IEnumerable<T> GetAll()
	{
		return _context.Set<T>().ToList();
	}

	public T GetById(int id)
	{
		return _context.Set<T>().Find(id);
	}

	public void Update(T model)
	{
		_context.Set<T>().Update(model);
		_context.SaveChanges();
	}
}
