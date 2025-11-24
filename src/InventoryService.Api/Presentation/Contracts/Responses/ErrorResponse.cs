namespace InventoryService.Api.Presentation.Contracts.Responses;

public record ErrorResponse(
    string Type,
    string Title,
    int Status,
    string Detail,
    string Instance,
    DateTimeOffset Timestamp
);