namespace ExerciseTracker.Forser.UI
{
    public static class Helpers
    {
        internal static Style HighLightStyle => new(
            Color.Orange1,
            Color.Black,
            Decoration.None
            );
        internal static void RenderTitle(string title)
        {
            Rule rule = new Rule($"[orange1]{title}[/]");
            AnsiConsole.Write(rule);
        }
        public static SelectionPrompt<MenuView> DisplayMainMenu()
        {
            AnsiConsole.Clear();
            RenderTitle("Main Menu");

            SelectionPrompt<MenuView> menu = new()
            {
                HighlightStyle = HighLightStyle
            };

            menu.Title("Select an [B]option[/]");
            menu.AddChoices(new List<MenuView>()
            {
                new() { Id = 0, Text = "Display Exercises" },
                new() { Id = 1, Text = "Add Exercise" },
                new() { Id = 2, Text = "Delete Exercise" },
                new() { Id = 3, Text = "Edit Exercise" },
                new() { Id = -1, Text = "Quit" }
            });

            return menu;
        }
    }
}