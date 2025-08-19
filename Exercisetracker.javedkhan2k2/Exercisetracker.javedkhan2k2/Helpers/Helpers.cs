using Newtonsoft.Json;

namespace Exercisetacker;

internal static class Helpers
{
    internal static string[] ConvertToArray<T>(IEnumerable<T> items, Func<T, string> selector)
    {
        if (items == null)
        {
            return Array.Empty<string>();
        }
        return items.Select(selector).ToArray();
    }    

}

public static class ObjectDumper
{
    public static void Dump(object obj)
    {
        foreach (var prop in obj.GetType().GetProperties())
        {
            var value = prop.GetValue(obj);
            Console.WriteLine($"{prop.Name}: {value}");
        }
    }
}

public static class Pretty
{
    public static void Print<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        Console.WriteLine(json);
    }
}