using System;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public class MenuItemViewModel
{
    public string DisplayName { get; set; } = string.Empty; 
    public Enum Value { get; set; }

    public MenuItemViewModel(string displayName, Enum value)
    {
        DisplayName = displayName;
        Value = value;
    }
}
