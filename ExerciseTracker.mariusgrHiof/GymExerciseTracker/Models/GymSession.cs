namespace GymExerciseTracker.Models;
public class GymSession
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan Duration => EndDate - StartDate;
    public string Comments { get; set; } = string.Empty;
}

