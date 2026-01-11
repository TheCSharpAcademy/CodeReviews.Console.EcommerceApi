using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace ECommerceApp.RyanW84.Controllers;

public static class ControllerResponseExtensions
{
    public static IActionResult FromFailure(this ControllerBase controller, HttpStatusCode statusCode, string errorMessage)
    {
        if (statusCode == HttpStatusCode.NoContent)
        {
            return controller.NoContent();
        }

        var detail = string.IsNullOrWhiteSpace(errorMessage)
            ? "Request could not be completed."
            : errorMessage;

        var status = (int)statusCode;
        return controller.Problem(
            title: ReasonPhrases.GetReasonPhrase(status),
            detail: detail,
            statusCode: status
        );
    }
}
