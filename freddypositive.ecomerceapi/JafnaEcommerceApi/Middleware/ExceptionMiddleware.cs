using JafnaEcommerceApi.Exceptions;
using Newtonsoft.Json;
using JafnaEcommerceApi.Models.DTOs.APIResponse;

namespace JafnaEcommerceApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        int statusCode = 500;
        string message = "An unexpected error occurred.";

        if (ex is AppException appEx)
        {
            statusCode = appEx.StatusCode;
            message = appEx.Message;
        }

        if (Environment.GetEnvironmentVariable("ENABLE_LOGGING") == "true"
            || context.RequestServices.GetService<IWebHostEnvironment>().IsDevelopment())
        {
            _logger.LogError(ex,
                "Error occurred on {Method} {Path}. StatusCode: {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                statusCode);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<object>
        {
            success = false,
            message = message,
            errors = new List<string> { message } // Or add stack trace if needed
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
