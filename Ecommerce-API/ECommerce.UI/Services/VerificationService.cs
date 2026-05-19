using ECommerce.Shared;
using ECommerce.UI.Interfaces;

namespace ECommerce.UI.Services;

public class VerificationService : IVerificationService
{
    public bool TryValidateItemPrice(string input, out decimal itemPrice, out string? errorMessage)
    {
        if (!decimal.TryParse(input, out itemPrice))
        {
            errorMessage = "Could not parse item price into a number.";
            return false;
        }
        
        if (itemPrice <= 0)
        {
            errorMessage = "Item price must be greater than or equal to 1.";
            return false;
        }
        
        errorMessage = null;
        return true;
    }

    public bool TryParseValidQuantity(string quantity, out int parsedQuantity)
    {
        return int.TryParse(quantity, out parsedQuantity) && parsedQuantity > 0;
    }

    public bool TryParseItemFormat(string input, out ItemFormat itemFormat)
    {
        return Enum.TryParse(input, true, out itemFormat);
    }

    public bool TryParseItemType(string input, out ItemType itemType)
    {
        return Enum.TryParse(input, true, out itemType);
    }
}