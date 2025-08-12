namespace GymExerciseTracker.Dtos;
public class UpdateGymSessionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Comments { get; set; } = string.Empty;
}

