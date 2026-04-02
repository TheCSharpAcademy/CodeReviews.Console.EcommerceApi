using ECommerce.Shared.TerrenceLGee.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;

public class SaleData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

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

    [JsonPropertyName("saleProducts")]
    public List<SaleProductData> SaleProducts { get; set; } = [];

    public string? ErrorMessage { get; set; }
}