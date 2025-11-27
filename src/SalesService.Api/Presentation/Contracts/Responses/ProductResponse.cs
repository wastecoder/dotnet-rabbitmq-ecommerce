namespace SalesService.Api.Presentation.Contracts.Responses;

public record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity,
    string? Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);