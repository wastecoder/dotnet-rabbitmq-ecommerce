using System.Net;
using FluentValidation;
using InventoryService.Api.Domain.Enums;
using InventoryService.Api.Domain.Exceptions;
using InventoryService.Api.Presentation.Contracts.Responses;
using InventoryService.Api.Presentation.Factories;

namespace InventoryService.Api.Presentation.Middlewares;

public class GlobalExceptionHandler(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        ErrorResponse response;
        int statusCode;

        switch (ex)
        {
            // FluentValidation Exceptions
            case ValidationException validationEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                response = ErrorResponseFactory.Create(
                    context,
                    ErrorType.ValidationError,
                    title: "Validation Error",
                    status: statusCode,
                    errors: validationEx.Errors.Select(e => e.ErrorMessage)
                );
                break;

            // Unauthorized
            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                response = ErrorResponseFactory.Create(
                    context,
                    ErrorType.Unauthorized,
                    title: "Unauthorized",
                    status: statusCode,
                    detail: "You are not authorized to perform this action."
                );
                break;

            // Forbidden
            case AccessViolationException:
                statusCode = (int)HttpStatusCode.Forbidden;
                response = ErrorResponseFactory.Create(
                    context,
                    ErrorType.Forbidden,
                    title: "Forbidden",
                    status: statusCode,
                    detail: "You do not have permission to access this resource."
                );
                break;

            // NotFoundException
            case NotFoundException notFoundEx:
                statusCode = (int)HttpStatusCode.NotFound;
                response = ErrorResponseFactory.Create(
                    context,
                    ErrorType.NotFound,
                    title: "Not Found",
                    status: statusCode,
                    detail: notFoundEx.Message
                );
                break;

            // Conflict
            case InvalidOperationException invalidOpEx:
                statusCode = (int)HttpStatusCode.Conflict;
                response = ErrorResponseFactory.Create(
                    context,
                    ErrorType.Conflict,
                    title: "Conflict",
                    status: statusCode,
                    detail: invalidOpEx.Message
                );
                break;

            // Other unhandled exceptions (500)
            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                response = ErrorResponseFactory.Create(
                    context,
                    ErrorType.InternalServerError,
                    title: "Internal Server Error",
                    status: statusCode,
                    detail: "An unexpected error occurred."
                );
                break;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(response);
    }
}