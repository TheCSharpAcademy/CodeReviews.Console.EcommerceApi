using ExerciseTracker.UI.Models;
using Spectre.Console;

namespace ExerciseTracker.UI;

public static class UserOutputs<T> where T : class
{
    public static void ShowResponse(ResponseDto<T> Response)
    {
        Console.Clear();
        if (!Response.IsSuccess)
        {
            string ResponseString = $"[yellow]Response Method:{Response.ResponseMethod}\n[maroon]Reponse Message:{Response.Message}[/][/]";
            var panel = new Panel(ResponseString);
            panel.Header = new PanelHeader("[Red]Request Failed!!![/]");
            panel.Border = BoxBorder.Rounded;
            panel.Padding = new Padding(2, 2, 2, 2);
            AnsiConsole.Write(panel);
            Console.ReadLine();
        }
        else
        {
            string ResponseString = $"[yellow]Response Method:{Response.ResponseMethod}\n[green]Reponse Message:{Response.Message}[/][/]";
            var panel = new Panel(ResponseString);
            panel.Header = new PanelHeader("[lime]Request Success!!![/]");
            panel.Border = BoxBorder.Rounded;
            panel.Padding = new Padding(2, 2, 2, 2);
            AnsiConsole.Write(panel);
            string Heading = Response.ResponseMethod switch
            {
                "GET" => "Here is the Entity Details",
                "POST" => "Details of the Entity Created",
                "PUT" => "Details of the updated Entity",
                "DELETE" => "Details of the Entity Deleted",
                _ => "Unknown"
            };
            if (Response.Data.Count() == 0)
            {
                ResponseString = $"[yellow]Response Method:{Response.ResponseMethod}\nCurrently No Data Found For the requested Entityy in DataBase[/]";
                var EmptyMessagePanel = new Panel(ResponseString);
                EmptyMessagePanel.Header = new PanelHeader("[lime]Empty Data!!![/]");
                EmptyMessagePanel.Border = BoxBorder.Rounded;
                EmptyMessagePanel.Padding = new Padding(2, 2, 2, 2);
                AnsiConsole.Write(EmptyMessagePanel);
                Console.ReadLine();
            }
            else
            {
                AnsiConsole.MarkupLine(Heading);
                Table Responsetable = new Table();
                Responsetable.Title = new TableTitle(typeof(T).Name);
                var props = typeof(T).GetProperties().ToList(); 
                props.ForEach(x => Responsetable.AddColumn(Markup.Escape(x.Name)));
                foreach (var ResponseObject in Response.Data)
                {
                    List<string> RowData = new();
                    props.ForEach(x => RowData.Add(x.GetValue(ResponseObject).ToString()));
                    Responsetable.AddRow(RowData.ToArray());
                }
                Responsetable.Border = TableBorder.Double;
                AnsiConsole.Write(Responsetable);
            }
        }
        Console.ReadLine();
    }
}
