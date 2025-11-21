namespace InventoryService.Api.Presentation.Contracts.Requests;

public record ProductRequest(
    string Name,
    decimal Price,
    int Quantity,
    string? Description
);