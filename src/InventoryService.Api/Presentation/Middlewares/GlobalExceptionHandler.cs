using FluentValidation;
using InventoryService.Api.Domain.Enums;
using InventoryService.Api.Domain.Exceptions;
using InventoryService.Api.Presentation.Factories;
using Microsoft.AspNetCore.Diagnostics;

namespace InventoryService.Api.Presentation.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int statusCode;
        ErrorType errorType;
        string title;
        string? detail = null;
        IEnumerable<string>? errors = null;

        switch (exception)
        {
            // FluentValidation
            case ValidationException validationEx:
                statusCode = StatusCodes.Status400BadRequest;
                errorType = ErrorType.ValidationError;
                title = "Validation Error";
                errors = validationEx.Errors.Select(e => e.ErrorMessage);
                break;

            // OutOfStock
            case OutOfStockException outOfStockEx:
                statusCode = StatusCodes.Status400BadRequest;
                errorType = ErrorType.BadRequest;
                title = "Out of Stock";
                detail = outOfStockEx.Message;
                break;

            // Unauthorized
            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                errorType = ErrorType.Unauthorized;
                title = "Unauthorized";
                detail = "You are not authorized to perform this action.";
                break;

            // Forbidden
            case AccessViolationException:
                statusCode = StatusCodes.Status403Forbidden;
                errorType = ErrorType.Forbidden;
                title = "Forbidden";
                detail = "You do not have permission to access this resource.";
                break;

            // NotFoundException
            case NotFoundException notFoundEx:
                statusCode = StatusCodes.Status404NotFound;
                errorType = ErrorType.NotFound;
                title = "Not Found";
                detail = notFoundEx.Message;
                break;

            // Conflict
            case InvalidOperationException invalidOpEx:
                statusCode = StatusCodes.Status409Conflict;
                errorType = ErrorType.Conflict;
                title = "Conflict";
                detail = invalidOpEx.Message;
                break;

            // Other unhandled exceptions (500)
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                errorType = ErrorType.InternalServerError;
                title = "Internal Server Error";
                detail = "An unexpected error occurred. Please try again later.";
                break;
        }

        var response = ErrorResponseFactory.Create(
            context,
            errorType,
            title,
            statusCode,
            detail,
            errors
        );

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}