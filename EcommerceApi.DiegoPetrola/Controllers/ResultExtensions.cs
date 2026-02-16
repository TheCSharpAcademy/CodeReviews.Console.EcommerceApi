using EcommerceApi.Models;
using EcommerceApi.Models.DTOs;
using EcommerceApi.Results;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers;

public static class ResultExtensions
{
    public static ActionResult ToActionResult<T>(this ControllerBase controller, Result<T> res)
    {
        if (res.IsSuccess)
            return controller.Ok(res.Value);

        return res.Error.ErrorType switch
        {
            ErrorType.NotFound => controller.NotFound(res.Error.Error),
            ErrorType.Invalid => controller.BadRequest(res.Error.Error),
            _ => controller.Problem(res.Error.Error)
        };
    }

    public static SaleDto ToSaleDto(Sale sale)
    {
        return new SaleDto(
            sale.Id,
            sale.SaleDate,
            [.. sale.SaleItems.Select(si => new SaleItemDto(
                        si.ProductId,
                        si.Product.Name,
                        si.Quantity,
                        si.Product.Price))],
            sale.SaleItems.Sum(si => si.Quantity * si.Product.Price)
        );
    }
}