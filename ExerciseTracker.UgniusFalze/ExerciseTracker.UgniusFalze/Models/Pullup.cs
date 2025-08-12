namespace ExerciseTracker.UgniusFalze.Models;

public class Pullup
{
    public int PullupId { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string Comment { get; set; }
    public int Repetitions { get; set; }

    public TimeSpan Duration => DateEnd - DateStart;
}