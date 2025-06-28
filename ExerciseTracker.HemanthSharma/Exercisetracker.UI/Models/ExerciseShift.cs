using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExerciseTracker.UI.Models
{
    public class ExerciseShift
    {
        [property: JsonPropertyName("id")]
        public int? Id { get; set; }
        [property: JsonPropertyName("exerciseId")]
        public int ExerciseId { get; set; }
        [property: JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }
        [property: JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }
        [property: JsonPropertyName("exerciseDate")]
        public DateTime ExerciseDate { get; set; }
        [property: JsonPropertyName("duration")]
        public int? Duration { get; set; }
        [property: JsonPropertyName("comments")]
        public string? Comments { get; set; }
       
    }
}
