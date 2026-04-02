using System.Text.Json.Serialization;

namespace ECommerce.Shared.TerrenceLGee.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SaleStatus
{
    Pending = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Canceled = 5,
    None
}