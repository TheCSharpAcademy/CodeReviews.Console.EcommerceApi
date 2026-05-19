namespace ECommerce.UI.Configuration;

internal class ApiUris
{
    internal static readonly string ItemRequestUri = $"{ApiSettings.BaseUrl}items";
    internal static readonly string TagRequestUri = $"{ApiSettings.BaseUrl}tags";
    internal static readonly string SaleRequestUri = $"{ApiSettings.BaseUrl}sales";
}