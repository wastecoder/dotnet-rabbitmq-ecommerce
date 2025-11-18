using Gateway.Api.Presentation.Models;

namespace Gateway.Api.Presentation.Factories;

public static class ProblemDetailsFactory
{
    public static ProblemDetailsDto Create(
        HttpContext context,
        ErrorType type,
        string title,
        int status,
        string detail)
    {
        return new ProblemDetailsDto(
            Type: type,
            Title: title,
            Status: status,
            Detail: detail,
            Instance: context.Request.Path,
            Timestamp: DateTimeOffset.UtcNow
        );
    }
}