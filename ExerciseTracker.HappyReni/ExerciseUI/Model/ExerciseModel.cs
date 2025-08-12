using System.Text.Json.Serialization;

namespace ExerciseUI.Model
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public TimeSpan Duration => DateEnd - DateStart;
        public string Comments { get; set; }
    
    }
}
