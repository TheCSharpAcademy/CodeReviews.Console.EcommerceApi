using ExerciseTracker.Data;

namespace ExerciseTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExerciceContext context = new();
            // Comment EnsureDeleted() out if no data reset needed
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Menus menus = new(context);
            menus.MainMenu();
        }
    }
}