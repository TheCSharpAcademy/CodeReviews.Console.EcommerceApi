using ECommerce.Shared.TerrenceLGee.Enums;

namespace ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;

public class SaleParams : BaseParams
{
    public int SaleId { get; set; }
    public SaleStatus SaleStatus { get; set; }
}