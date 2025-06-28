using System.Text.Json.Serialization;

namespace ExerciseTracker.UI.Models;

public class ExerciseShiftDto
{
    [property: JsonPropertyName("id")]
    public int? Id { get; set; }
    [property: JsonPropertyName("exerciseId")]
    public int ExerciseId { get; set; }
    [property: JsonPropertyName("startTime")]
    public string StartTime { get; set; }
    [property: JsonPropertyName("endTime")]
    public string EndTime { get; set; }
    [property: JsonPropertyName("exerciseDate")]
    public string ExerciseDate { get; set; }
    [property: JsonPropertyName("comments")]
    public string? Comments { get; set; }
}
