using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace Ecommerce.Web.Extensions;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            ApplicationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };
        
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        
        var safeErrorMessage = statusCode == (int)HttpStatusCode.InternalServerError 
            ? "An unexpected error occurred on the server. Please try again later."
            : exception.Message;
        
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new()
            {
                Status = statusCode,
                Type = exception.GetType().Name,
                Title = "Internal Server Error",
                Detail = safeErrorMessage
            }
        });
    }
}