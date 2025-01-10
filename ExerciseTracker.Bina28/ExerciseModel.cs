namespace ExerciseTracker;

internal class ExerciseModel
{
	public int Id { get; set; }
	public DateTime DateStart { get; set; }
	public DateTime DateEnd { get; set; }
	public string Comments { get; set; } = string.Empty;
	public TimeSpan Duration => DateEnd - DateStart;
}
