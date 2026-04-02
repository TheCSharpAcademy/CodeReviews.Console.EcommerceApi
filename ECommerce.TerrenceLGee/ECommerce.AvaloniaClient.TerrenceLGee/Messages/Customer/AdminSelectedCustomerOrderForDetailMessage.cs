using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Messages.Customer;

public record AdminSelectedCustomerOrderForDetailMessage(int SaleId, CustomerData Data);
