using ExerciseTracker.UI.Models;
using ExerciseTracker.UI.Repositories;
using Spectre.Console;

namespace ExerciseTracker.UI
{
    public class UserInputs<T> where T : class
    {
        internal static int? GetExerciseById(List<Exercise> Entities)
        {
            List<string> UserChoices = new();

            UserChoices = Entities.Select(x => x.name).ToList();

            var UserOption = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Please Choose an option")
                .AddChoices(UserChoices));
            return Entities.Where(x => x.name == UserOption).Select(x => x.id).FirstOrDefault();
        }
        internal static int GetShiftById(List<ExerciseShiftDto> Entities)
        {
            if (Entities.Count() == 0)
            {
                AnsiConsole.MarkupLine("[pink3] No Entities Found!!![/]");
                Console.WriteLine("Click any key to Continue");
                Console.ReadLine();
                return -1;
            }
            List<string> UserChoices = new();
            UserChoices = Entities.Select(x => $"ShiftId={x.Id};ShiftDate={x.ExerciseDate};ShiftStartTime={x.StartTime}").ToList();
            var UserOption = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Please Choose an option")
                .AddChoices(UserChoices));
            return int.Parse(UserOption.Split(";")[0].Split("=")[1]);
        }

        internal static Exercise GetNewExercise()
        {
            string Name = AnsiConsole.Ask<string>("[yellow]Enter the [Blue]Name[/] of the Exercise you want to Add:[/]");
            return new Exercise
            {
                name = Name
            };
        }

        internal static T GetUpdatedEntity(List<T> UpdatedList)
        {
            T UpdatedEntity = Activator.CreateInstance<T>();
            var props = typeof(T).GetProperties();
            string UpdatedValue = "";
            foreach (var prop in props)
            {
                string PropertyName = prop.Name;
                if (PropertyName.ToLower() != "id")
                {
                    var res = AnsiConsole.Confirm($"[fuchsia]Do you want to change the[yellow] {prop.Name.ToString()}[/] Property:? The Current Value is [aqua]{prop.GetValue(UpdatedList[0])}[/][/]");
                    if (res)
                    {
                        if (PropertyName.ToLower() == "exerciseid")
                        {
                            Repository<Exercise> Repo = new();

                            int? ExerciseId = UserInputs<Exercise>.GetExerciseById(Repo.GetAllEntities().GetAwaiter().GetResult().Data);
                            UpdatedValue = ExerciseId.ToString();

                        }
                        else if (PropertyName.ToLower() == "starttime")
                        {
                            UpdatedValue = UserInputs<ExerciseShiftDto>.GetShiftTime().ToString("HH:mm");
                        }
                        else if (PropertyName.ToLower() == "endtime")
                        {
                            UpdatedValue = UserInputs<ExerciseShiftDto>.GetShiftTime(DateTime.Parse(UpdatedValue)).ToString("HH:mm");
                        }
                        else if (PropertyName.ToLower() == "exercisedate")
                        {
                            UpdatedValue = UserInputs<ExerciseShiftDto>.GetShiftDate().ToString("dd-MM-yyyy");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[olive] Enter the Updated Value:[/]");
                            UpdatedValue = Console.ReadLine();
                        }

                    }
                    else
                    {
                        UpdatedValue = prop.GetValue(UpdatedList[0]).ToString();
                    }
                    if (prop.Name.ToLower() == "exerciseid")
                    {
                        prop.SetValue(UpdatedEntity, int.Parse(UpdatedValue));
                    }
                    else
                    {
                        prop.SetValue(UpdatedEntity, UpdatedValue);
                    }
                }
            }
            return UpdatedEntity;
        }

        internal static ExerciseShiftDto GetNewShift(List<Exercise> Exercises)
        {
            AnsiConsole.MarkupLine("[cyan1]Choose the Exercise for which Shift is to be Created:[/]");
            //Table Responsetable = new Table();
            //Responsetable.Title = new TableTitle("[lightseagreen]Available Exercises[/]");
            //Responsetable.AddColumn("Id");
            //Responsetable.AddColumn("Name");
            //foreach (var Exercise in Exercises)
            //{   
            //    Responsetable.AddRow(Exercise.id.ToString(),Exercise.name.ToString());
            //}
            //Responsetable.Border = TableBorder.Double;
            //AnsiConsole.Write(Responsetable);
            List<string> ExerciseNames = Exercises.Select(x => $"Id:{x.id} ExerciseName:{x.name}").ToList();
            string Userchoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Please select an Exercise for the Shift:")
                .AddChoices(ExerciseNames));
            int ExerciseId = int.Parse(Userchoice.Split(" ")[0].Split(":")[1]);
            AnsiConsole.MarkupLine("[yellow3] Enter startTime of Shift[/]");
            DateTime StartTime = GetShiftTime();
            AnsiConsole.MarkupLine("[yellow3] Enter EndTime of Shift[/]");
            DateTime EndTime = GetShiftTime(StartTime);
            DateTime ExerciseDate = GetShiftDate();
            AnsiConsole.MarkupLine("comments for this shift?");
            string? Comments = Console.ReadLine();
            return new ExerciseShiftDto
            {
                ExerciseId = ExerciseId,
                StartTime = StartTime.ToString("HH:mm"),
                EndTime = EndTime.ToString("HH:mm"),
                ExerciseDate = ExerciseDate.ToString("dd-MM-yyyy"),
                Comments = Comments
            };
        }

        private static DateTime GetShiftDate()
        {
            bool res = AnsiConsole.Confirm("[orange4] Do you want to Enter Custom date?[/][chartreuse2](Default value will be Today's Date)[/]");
            DateTime ExerciseDate;
            if (res)
            {
                ExerciseDate = Validations.GetValidDate();
            }
            else
            {
                ExerciseDate = DateTime.Now.Date;
            }
            return ExerciseDate;
        }

        private static DateTime GetShiftTime(DateTime? StartTime = null)
        {
            DateTime StartTimeValue = DateTime.Now;
            bool isTimeValid = true;
            if (StartTime != null)
            {
                StartTimeValue = (DateTime)StartTime;
                isTimeValid = false;

            }
            DateTime TimeResult = Validations.GetValidTime();
            while (!isTimeValid)
            {
                if (TimeResult >= StartTime)
                {
                    isTimeValid = true;
                }
                else
                {
                    AnsiConsole.MarkupLine($"[lightgreen]The StartTime of the Shift is {StartTimeValue.ToShortTimeString()} EndDate Can't be before StartTime [/]");
                    TimeResult = Validations.GetValidTime();
                }
            }
            return TimeResult;
        }
    }
}
