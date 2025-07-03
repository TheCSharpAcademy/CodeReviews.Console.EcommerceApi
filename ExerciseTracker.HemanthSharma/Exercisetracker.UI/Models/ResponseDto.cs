using System.Text.Json.Serialization;

namespace ExerciseTracker.UI.Models;

public class ResponseDto<T> where T : class
{
    [property: JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }
    [property: JsonPropertyName("responseMethod")]
    public string ResponseMethod { get; set; }
    [property: JsonPropertyName("message")]
    public string Message { get; set; }
    [property: JsonPropertyName("data")]
    public List<T>? Data { get; set; }
}
