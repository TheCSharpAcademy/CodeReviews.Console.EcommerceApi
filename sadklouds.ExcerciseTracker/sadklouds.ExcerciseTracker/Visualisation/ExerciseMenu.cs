namespace sadklouds.ExcerciseTracker.Visualisation;
public static class ExerciseMenu
{
    public static void Menu()
    {
        Console.WriteLine("-------------Exercise Tracker------------");
        Console.WriteLine("V) View All Exercises");
        Console.WriteLine("A) Add Exercise");
        Console.WriteLine("I) Insepct single Exercise");
        Console.WriteLine("D) Delete Exercise");
        Console.WriteLine("U) Update Exercise");
        Console.WriteLine("0) Exit application");
        Console.WriteLine("-----------------------------------------");
        Console.Write("Please select and option: ");
    }
}
