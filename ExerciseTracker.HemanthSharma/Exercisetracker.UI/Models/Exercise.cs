using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExerciseTracker.UI.Models
{
    public class Exercise
    {
        [property:JsonPropertyName("id")]
        public int? id { get; set; }
        [property: JsonPropertyName("name")]
        public string name { get; set; }
    }
}
