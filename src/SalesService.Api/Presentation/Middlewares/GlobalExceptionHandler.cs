using FluentValidation;
using SalesService.Api.Domain.Enums;
using SalesService.Api.Presentation.Factories;
using Microsoft.AspNetCore.Diagnostics;
using SalesService.Api.Domain.Exceptions;

namespace SalesService.Api.Presentation.Middlewares;

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

            // Not Found
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

            // Business validation error
            case BusinessValidationException businessValidationEx:
                statusCode = StatusCodes.Status400BadRequest;
                errorType = ErrorType.BadRequest;
                title = "Business Validation Error";
                detail = businessValidationEx.Message;
                break;

            // Upstream/External service failure
            case ExternalServiceException externalServiceEx:
                statusCode = StatusCodes.Status502BadGateway;
                errorType = ErrorType.UpstreamServiceError;
                title = "External Service Error";
                detail = externalServiceEx.Message;
                break;

            // Failure when calling an external service
            case HttpRequestException httpEx:
                statusCode = StatusCodes.Status502BadGateway;
                errorType = ErrorType.UpstreamServiceError;
                title = "External Service Unavailable";
                detail = "Failed to reach external service. Please try again later.";
                break;

            // Default
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