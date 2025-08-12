using System.ComponentModel;

namespace ExerciseTracker.frockett.UI;

public enum MainMenuOptions
{
    [Description("View All Exercise Sessions")]
    ViewSessions,
    [Description("Add a Session")]
    AddSession,
    [Description("Delete a Session")]
    DeleteSession,
    [Description("Update a Session")]
    UpdateSession,
    [Description("Exit")]
    Exit
}
