using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Serilog;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Log semua exception
        _logger.LogError(exception, "Unhandled exception occurred: {ExceptionMessage}", exception.Message);

        // Tentukan Status Code berdasarkan tipe Exception
        var statusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status422UnprocessableEntity, // Business logic error
            _ => StatusCodes.Status500InternalServerError
        };

        // Inisialisasi ProblemDetails dasar (Standar RFC 7807)
        var problemDetails = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = statusCode switch
            {
                404 => "Resource Not Found",
                400 => exception is ValidationException ? "Validation Error" : "Bad Request",
                422 => "Unprocessable Entity",
                _ => "Internal Server Error"
            },
            Status = statusCode,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        // Khusus untuk ValidationException (FluentValidation)
        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.Select(f => f.ErrorMessage).ToArray()
                );
        }

        // Set Response Header & Body sesuai standar Problem Details
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // Exception telah ditangani
    }
}