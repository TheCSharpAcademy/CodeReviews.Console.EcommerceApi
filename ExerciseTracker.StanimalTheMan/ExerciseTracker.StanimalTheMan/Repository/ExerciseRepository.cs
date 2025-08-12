using ExerciseTracker.StanimalTheMan.Data;
using ExerciseTracker.StanimalTheMan.Models;

namespace ExerciseTracker.StanimalTheMan.Repository;

public class ExerciseRepository : IExerciseRepository
{
	private readonly ExerciseContext _context;

	public ExerciseRepository(ExerciseContext context)
	{
		_context = context;
	}

	public void AddExercise(Run run)
	{
		_context.RunEntries.Add(run);
		_context.SaveChanges();
	}

	public void DeleteExercise(int id)
	{
		var run = _context.RunEntries.Find(id);
		if (run != null)
		{
			_context.RunEntries.Remove(run);
			_context.SaveChanges();
			Console.WriteLine($"Run entry {id} was succesfully deleted.");
		}
		else
		{
			Console.WriteLine($"Run entry with ID {id} does not exist.");
		}
	}

	public IEnumerable<Run> GetAllExercises()
	{
		return _context.RunEntries.ToList();
	}

	public Run GetExerciseById(int id)
	{
		return _context.RunEntries.FirstOrDefault(run => run.Id == id);
	}

	public void UpdateExercise(Run updatedRun)
	{
		var currentRun = _context.RunEntries.Find(updatedRun.Id);

		if (currentRun != null)
		{
			currentRun.Distance = updatedRun.Distance;
			currentRun.DateStart = updatedRun.DateStart;
			currentRun.DateEnd = updatedRun.DateEnd;
			currentRun.Duration = updatedRun.Duration;
			currentRun.Comments = updatedRun.Comments;

			_context.SaveChanges();
			Console.WriteLine($"Run {updatedRun.Id} was successfully updated.");
		}
		else
		{
			Console.WriteLine($"Run with ID {updatedRun.Id} does not exist.");
		}
	}
}
