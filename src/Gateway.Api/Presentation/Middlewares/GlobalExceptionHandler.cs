using Gateway.Api.Domain.Exceptions;
using Gateway.Api.Presentation.Factories;
using Gateway.Api.Presentation.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace Gateway.Api.Presentation.Middlewares;

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
        string detail;

        switch (exception)
        {
            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                errorType = ErrorType.Unauthorized;
                title = "Unauthorized";
                detail = "You are not authorized to perform this action.";
                break;

            case ForbiddenAccessException:
                statusCode = StatusCodes.Status403Forbidden;
                errorType = ErrorType.Forbidden;
                title = "Forbidden";
                detail = "Access to this resource is forbidden.";
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                errorType = ErrorType.InternalServerError;
                title = "Internal Server Error";
                detail = "An unexpected error occurred. Please try again later.";
                break;
        }

        var problem = ProblemDetailsFactory.Create(
            context,
            errorType,
            title,
            statusCode,
            detail
        );

        context.Response.StatusCode = problem.Status;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}