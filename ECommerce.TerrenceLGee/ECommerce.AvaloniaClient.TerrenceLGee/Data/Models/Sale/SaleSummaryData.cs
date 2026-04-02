using ECommerce.Shared.TerrenceLGee.Enums;
using System;
using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;

public class SaleSummaryData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("saleProductCount")]
    public int SaleProductCount { get; set; }

    [JsonPropertyName("totalBaseAmount")]
    public double TotalBaseAmount { get; set; }

    [JsonPropertyName("totalDiscountAmount")]
    public double TotalDiscountAmount { get; set; }

    [JsonPropertyName("totalAmount")]
    public double TotalAmount { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("saleStatus")]
    public SaleStatus SaleStatus { get; set; }
}