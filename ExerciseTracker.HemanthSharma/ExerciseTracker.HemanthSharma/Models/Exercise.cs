using ExerciseTracker.HemanthSharma.Interfaces;
using System.Text.Json.Serialization;

namespace ExerciseTracker.Study.Models;

public class Exercise : IEntity<Exercise>
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public List<ExerciseShift>? ExerciseShifts { get; set; }
}
