using ConsoleTableExt;
using ExerciseTracker.Dtos;

namespace ExerciseTracker.Visualization;

public class TableVisualization : ITableVisualization
{
    public void DisplayTable(List<ExerciseViewDto> exercises)
    {
        Console.Clear();
        ConsoleTableBuilder
            .From(exercises)
            .ExportAndWriteLine();
    }
}