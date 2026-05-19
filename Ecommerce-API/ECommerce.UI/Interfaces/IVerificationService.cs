using ECommerce.Shared;

namespace ECommerce.UI.Interfaces;

public interface IVerificationService
{
    public bool TryParseItemFormat(string input, out ItemFormat itemFormat);
    public bool TryParseItemType(string input, out ItemType itemType);
    public bool TryValidateItemPrice(string input, out decimal itemPrice, out string? errorMessage);
    public bool TryParseValidQuantity(string quantity, out int parsedQuantity);
}