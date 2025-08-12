using ExerciseTracker.Dtos;

namespace ExerciseTracker.Visualization;

public interface ITableVisualization
{
    public void DisplayTable(List<ExerciseViewDto> exercises);
}