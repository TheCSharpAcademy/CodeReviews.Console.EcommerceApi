namespace ExerciseTracker.iamnikitakostin.Models;
public class Exercise
{
    public int Id { get; set; }
    public string Comments { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration { get; set; }
}
