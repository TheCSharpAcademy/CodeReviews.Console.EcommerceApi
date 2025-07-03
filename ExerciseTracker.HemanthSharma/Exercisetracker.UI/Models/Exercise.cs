using System.Text.Json.Serialization;

namespace ExerciseTracker.UI.Models;

public class Exercise
{
    [property: JsonPropertyName("id")]
    public int? id { get; set; }
    [property: JsonPropertyName("name")]
    public string name { get; set; }
}
