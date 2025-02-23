using ExerciseTracker.Arashi256.Classes;
using ExerciseTracker.Arashi256.Config;
using ExerciseTracker.Arashi256.Controllers;
using ExerciseTracker.Arashi256.Enums;
using ExerciseTracker.Arashi256.Models;
using Spectre.Console;

namespace ExerciseTracker.Arashi256.Views
{
    internal class MainView
    {
        private const int QUIT_APPLICATION_OPTION_NUM = 5;
        private readonly string DATETIME_FORMAT;
        private readonly Table _tblMainMenu;
        private readonly string _appTitle = "EXERCISE TRACKER";
        private readonly FigletText _figletAppTitle;
        private readonly string[] _menuOptions =
        {
            "Add New Exercise Session",
            "Update Existing Exercise Session",
            "Delete Existing Exercise Session",
            "List All Exercise Sessions",
            "Quit application"
        };
        private ExerciseSessionController _controller;

        public MainView(ExerciseSessionController controller, AppSettings appSettings)
        {
            _figletAppTitle = new FigletText(_appTitle);
            _figletAppTitle.Centered();
            _figletAppTitle.Color = Color.Yellow3_1;
            _tblMainMenu = new Table();
            _tblMainMenu.AddColumn(new TableColumn("[orange1]CHOICE[/]").Centered());
            _tblMainMenu.AddColumn(new TableColumn("[orange1]OPTION[/]").LeftAligned());
            for (int i = 0; i < _menuOptions.Length; i++)
            {
                _tblMainMenu.AddRow($"[white]{i + 1}[/]", $"[yellow]{_menuOptions[i]}[/]");
            }
            _tblMainMenu.Alignment(Justify.Center);
            _controller = controller;
            // Default to this datetime format if external settings can't be loaded.
            DATETIME_FORMAT = appSettings.DateTimeFormat ?? "dd-MM-yy HH:mm";
        }

        public void DisplayView()
        {
            int selectedValue = 0;
            do
            {
                Console.Clear();
                AnsiConsole.Write(_figletAppTitle);
                AnsiConsole.Write(new Text("M A I N   M E N U").Centered());
                AnsiConsole.Write(_tblMainMenu);
                selectedValue = CommonUI.MenuOption($"Enter a value between 1 and {_menuOptions.Length}: ", 1, _menuOptions.Length);
                ProcessMainMenu(selectedValue);
            } while (selectedValue != QUIT_APPLICATION_OPTION_NUM);
            AnsiConsole.MarkupLine("[lime]Goodbye![/]");
        }

        private void ProcessMainMenu(int option)
        {
            AnsiConsole.Markup($"[lightslategrey]Menu option selected: {option}[/]\n");
            switch (option)
            {
                case 1:
                    // Add new exercise session
                    AddNewExerciseSession();
                    break;
                case 2:
                    // Update an existing exercise session
                    UpdateExistingExerciseSession();
                    break;
                case 3:
                    // Delete existing exercise session
                    DeleteExistingExerciseSession();
                    break;
                case 4:
                    // List all exercise sessions
                    ListAllExerciseSessions(true);
                    CommonUI.Pause("grey53");
                    break;
            }
        }

        private void AddNewExerciseSession() 
        {
            ExerciseSessionInputDto? newSession = GetExerciseSessionDetails();
            if (newSession != null)
            {
                DisplayExerciseSessionInput(newSession);
                if (AnsiConsole.Confirm("Are you sure you want to add this exercise session?"))
                {
                    var response = _controller.AddNewExerciseSession(newSession);
                    if (response.Status.Equals(ResponseStatus.Success))
                        AnsiConsole.MarkupLine($"[green]New exercise session added: '{response.Message}'[/]");
                    else
                        AnsiConsole.MarkupLine($"[red]Exercise addition failed: '{response.Message}'[/]");
                }
                else
                    AnsiConsole.MarkupLine("\n[yellow]Operation cancelled[/]\n");
            }
            else
            {
                AnsiConsole.MarkupLine("\n[yellow]Operation cancelled[/]\n");
            }
            CommonUI.Pause("grey53");
        }
        private void UpdateExistingExerciseSession() 
        {
            List<ExerciseSessionOutputDto>? sessions = ListAllExerciseSessions(false);
            if (sessions != null)
            {
                int selectedSession = CommonUI.MenuOption("(Enter '0' to cancel)\nPlease select an exercise session ID to update: ", 0, sessions.Count);
                if (selectedSession == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]Operation cancelled[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"Selected session: {selectedSession}");
                    var updateSession = sessions[selectedSession - 1];
                    DisplayExerciseSessionOutput(updateSession);
                    if (AnsiConsole.Confirm("Are you sure you want to update this exercise session?"))
                    {
                        ExerciseSessionInputDto? newSession = GetExerciseSessionDetails();
                        if (newSession != null)
                        {
                            newSession.Id = updateSession.Id;
                            var response = _controller.UpdateExistingExerciseSession(updateSession.Id, newSession);
                            if (response.Status.Equals(ResponseStatus.Success))
                            {
                                AnsiConsole.MarkupLine($"[green]Exercise session updated: '{response.Message}'[/]");
                                var updatedSession = response.Data as ExerciseSessionOutputDto;
                                if (updatedSession != null)
                                {
                                    DisplayExerciseSessionOutput(updatedSession);
                                }
                                else
                                    AnsiConsole.MarkupLine("\n[yellow]Could not display updated exercise session[/]\n");
                            }
                            else
                                AnsiConsole.MarkupLine($"[red]Exercise session update failed: '{response.Message}'[/]");
                        }
                        else
                            AnsiConsole.MarkupLine("\n[yellow]Operation cancelled[/]\n");
                    }
                    else
                        AnsiConsole.MarkupLine("\n[yellow]Operation cancelled[/]\n");
                }
            }
            CommonUI.Pause("grey53");
        }

        private void DeleteExistingExerciseSession()
        {
            List<ExerciseSessionOutputDto>? sessions = ListAllExerciseSessions(false);
            if (sessions != null)
            {
                int selectedSession = CommonUI.MenuOption("(Enter '0' to cancel)\nPlease select an exercise session ID to delete: ", 0, sessions.Count);
                if (selectedSession == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]Operation cancelled[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"Selected session: {selectedSession}");
                    var deleteSession = sessions[selectedSession - 1];
                    DisplayExerciseSessionOutput(deleteSession);
                    if (AnsiConsole.Confirm("Are you sure you want to delete this exercise session?"))
                    {
                        var response = _controller.DeleteExistingExerciseSession(deleteSession.Id);
                        if (response.Status.Equals(ResponseStatus.Success))
                            AnsiConsole.MarkupLine($"[green]Exercise session deleted: '{response.Message}'[/]");
                        else
                            AnsiConsole.MarkupLine($"[red]Exercise session delete failed: '{response.Message}'[/]");
                    }
                    else
                        AnsiConsole.MarkupLine("\n[yellow]Operation cancelled[/]\n");
                }
            }
            CommonUI.Pause("grey53");
        }

        private ExerciseSessionInputDto? GetExerciseSessionDetails()
        {
            bool isValidSession = false;
            ExerciseType exerciseType;
            DateTime startDateTime, endDateTime;
            string? comments;
            do
            {
                // Get exercise types from the public enum.
                exerciseType = CommonUI.GetExerciseTypeDialog(new List<ExerciseType> { ExerciseType.HIKING, ExerciseType.CARDIO });
                AnsiConsole.MarkupLine($"\n[white]SELECTED: {exerciseType.ToString()}[/]");
                // Get session start
                AnsiConsole.Markup("\n[white]Enter the start of the exercise session[/]\n");
                startDateTime = CommonUI.GetDateTimeDialog(DATETIME_FORMAT) ?? DateTime.MinValue;
                if (startDateTime == DateTime.MinValue) return null;
                // Get session end
                AnsiConsole.Markup("\n[white]Enter the end of the exercise session[/]\n");
                endDateTime = CommonUI.GetDateTimeDialog(DATETIME_FORMAT) ?? DateTime.MinValue;
                if (endDateTime == DateTime.MinValue) return null;
                // Validate session datetimes
                isValidSession = Validation.ValidateDatesForDuration(startDateTime, endDateTime);
                // Any other comments
                comments = CommonUI.GetStringWithPrompt("Enter any other comments (enter 'n' for none): ", 255, "n");
                if (comments == null) return null;
                if (comments.ToLower().Equals("n")) comments = null;
                if (!isValidSession)
                    AnsiConsole.MarkupLine("\n[yellow]The exercise session end cannot be before session start. Try again.[/]\n");                
            } while (!isValidSession);
            var newSession = new ExerciseSessionInputDto
            {
                Type = exerciseType.ToString().ToUpper(),
                DateTimeStart = startDateTime,
                DateTimeEnd = endDateTime,
                Comments = comments
            };
            return newSession;
        }

        private void DisplayExerciseSessionInput(ExerciseSessionInputDto exerciseSession)
        {
            Table tblSession = new Table();
            tblSession.AddColumn(new TableColumn("[cyan]ID[/]").RightAligned());
            tblSession.AddColumn(new TableColumn($"[white]1[/]").LeftAligned());
            tblSession.AddRow($"[cyan]Type[/]", $"[white]{exerciseSession.Type.ToString().ToUpper()}[/]");
            tblSession.AddRow($"[cyan]Start Time[/]", $"[white]{exerciseSession.DateTimeStart.ToString(DATETIME_FORMAT)}[/]");
            tblSession.AddRow($"[cyan]End Time[/]", $"[white]{exerciseSession.DateTimeEnd.ToString(DATETIME_FORMAT)}[/]");
            tblSession.AddRow($"[cyan]Comments[/]", $"[white]{exerciseSession.Comments ?? "NONE"}[/]");
            AnsiConsole.Write(tblSession);
        }

        private void DisplayExerciseSessionOutput(ExerciseSessionOutputDto exerciseSession)
        {
            Table tblSession = new Table();
            tblSession.AddColumn(new TableColumn("[cyan]ID[/]").RightAligned());
            tblSession.AddColumn(new TableColumn($"[white]{exerciseSession.DisplayId}[/]").LeftAligned());
            tblSession.AddRow($"[cyan]Type[/]", $"[white]{exerciseSession.Type.ToString().ToUpper()}[/]");
            tblSession.AddRow($"[cyan]Start Time[/]", $"[white]{exerciseSession.DateTimeStart.ToString(DATETIME_FORMAT)}[/]");
            tblSession.AddRow($"[cyan]End Time[/]", $"[white]{exerciseSession.DateTimeEnd.ToString(DATETIME_FORMAT)}[/]");
            tblSession.AddRow($"[cyan]Duration[/]", $"[white]{exerciseSession.Duration.ToString(@"hh\:mm")}[/]");
            tblSession.AddRow($"[cyan]Comments[/]", $"[white]{exerciseSession.Comments ?? "NONE"}[/]");
            AnsiConsole.Write(tblSession);
        }

        private List<ExerciseSessionOutputDto>? ListAllExerciseSessions(bool showTotals)
        {
            var response = _controller.GetAllExerciseSessions();
            if (response.Status.Equals(ResponseStatus.Success))
            {
                List<ExerciseSessionOutputDto>? displaySessions = response.Data as List<ExerciseSessionOutputDto>;
                if (displaySessions != null && displaySessions.Count > 0)
                {
                    DisplayExerciseSessions(displaySessions, showTotals);
                    return displaySessions;
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]There are no exercise sessions to display[/]");
                }
            }
            else
                AnsiConsole.MarkupLine($"[red]{response.Message}[/]");
            return null;
        }

        private void DisplayExerciseSessions(List<ExerciseSessionOutputDto>? sessions, bool showTotal)
        {
            TimeSpan totalDuration = TimeSpan.Zero;
            if (sessions == null || !sessions.Any()) return;
            Table sessionsTable = new Table();
            sessionsTable.AddColumn(new TableColumn("[white]ID[/]").Centered());
            sessionsTable.AddColumn(new TableColumn("[white]Type[/]").LeftAligned());
            sessionsTable.AddColumn(new TableColumn("[white]Session Start[/]").LeftAligned());
            sessionsTable.AddColumn(new TableColumn("[white]Session End[/]").LeftAligned());
            sessionsTable.AddColumn(new TableColumn("[white]Comments[/]").LeftAligned());
            sessionsTable.AddColumn(new TableColumn("[white]Duration[/]").LeftAligned());
            sessionsTable.Alignment(Justify.Center);
            foreach (var session in sessions)
            {
                var displayId = session.DisplayId.ToString();
                var Type = session.Type.ToString().ToUpper() ?? "UNKNOWN";
                var sessionStart = session.DateTimeStart.ToString(DATETIME_FORMAT);
                var sessionEnd = session.DateTimeEnd.ToString(DATETIME_FORMAT);
                var sessionDuration = session.Duration.ToString(@"hh\:mm");
                var sesseionComments = session.Comments ?? "N/A";
                totalDuration += session.Duration;
                sessionsTable.AddRow(
                    displayId,
                    Type,
                    sessionStart,
                    sessionEnd,
                    sesseionComments,
                    sessionDuration
                );
            }
            if (showTotal)
            {
                sessionsTable.AddRow("");
                sessionsTable.AddRow("", "", "", "", "[yellow]Total Duration[/]", $"[cyan]{totalDuration.ToString(@"hh\:mm")}[/]");
            }
            AnsiConsole.Write(sessionsTable);
        }
    }
}
