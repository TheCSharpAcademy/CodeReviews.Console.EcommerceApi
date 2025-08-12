using ConsoleTableExt;
using ExerciseTracker.Speedierone.Model;

namespace ExerciseTracker.Speedierone
{
    internal class TableLayout
    {
        public static void DisplayTable(List<Exercises> sessions)
        {
            var tableData = new List<List<Object>>();
            foreach (Exercises exercises in sessions)
            {
                tableData.Add(new List<object>
                {
                    exercises.Id,
                    exercises.DateStart,
                    exercises.DateEnd,
                    exercises.Duration,
                    exercises.Comments
                });              
            }
            ConsoleTableBuilder.From(tableData).WithColumn("ID", "Start Date", "End Time", "Duration", "Comments").ExportAndWriteLine();
        }
        public static void DisplayTableAll(IEnumerable<Exercises> sessions)
        {
            Console.Clear();
            var tableData = new List<List<Object>>();
            foreach (Exercises exercises in sessions)
            {
                tableData.Add(new List<object>
                {
                    exercises.Id,
                    exercises.DateStart,
                    exercises.DateEnd,
                    exercises.Duration,
                    exercises.Comments
                });               
            }
            ConsoleTableBuilder.From(tableData).WithColumn("ID", "Start Date", "End Time", "Duration", "Comments").ExportAndWriteLine();
        }
    }
}
