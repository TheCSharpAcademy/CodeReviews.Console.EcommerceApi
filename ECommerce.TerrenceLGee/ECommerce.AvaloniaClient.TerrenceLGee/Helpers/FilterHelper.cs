using System;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Helpers;

public static class FilterHelper
{
    public static async Task OnFilterChangedAsync(int page, Func<Task> loadMethod)
    {
        await Task.Delay(500);
        page = 1;
        await loadMethod();
    }
}
