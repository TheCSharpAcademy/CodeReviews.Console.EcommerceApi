namespace ExerciseTracker.StanimalTheMan.Models;

public class Run
{
	public int Id { get; set; }
	public double Distance { get; set; }
	public DateTime DateStart { get; set; }
	public DateTime DateEnd { get; set; }
	public TimeSpan Duration { get; set; }
	public string Comments { get; set; }
}
