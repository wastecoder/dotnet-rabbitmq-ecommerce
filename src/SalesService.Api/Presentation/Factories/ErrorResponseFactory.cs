using SalesService.Api.Domain.Enums;
using SalesService.Api.Presentation.Contracts.Responses;

namespace SalesService.Api.Presentation.Factories;

public static class ErrorResponseFactory
{
    public static ErrorResponse Create(
        HttpContext context,
        ErrorType type,
        string title,
        int status,
        string? detail = null,
        IEnumerable<string>? errors = null
    )
    {
        return new ErrorResponse(
            Type: $"/errors/{GetEnumDescription(type)}",
            Title: title,
            Status: status,
            Detail: detail,
            Instance: context.Request.Path,
            Timestamp: DateTimeOffset.UtcNow,
            Errors: errors
        );
    }

    private static string GetEnumDescription(Enum value)
    {
        return string
            .Concat(value.ToString()
                .Select((x, i) => i > 0 && char.IsUpper(x) ? $"_{x}" : $"{x}") 
                .ToArray())
            .ToLower();
    }
}