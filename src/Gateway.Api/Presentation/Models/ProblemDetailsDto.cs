namespace Gateway.Api.Presentation.Models;

public record ProblemDetailsDto(
    ErrorType Type,
    string Title,
    int Status,
    string Detail,
    string Instance,
    DateTimeOffset Timestamp
);