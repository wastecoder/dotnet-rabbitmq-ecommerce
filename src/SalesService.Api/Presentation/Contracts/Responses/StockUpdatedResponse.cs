namespace SalesService.Api.Presentation.Contracts.Responses;

public record StockUpdatedResponse(
    Guid ProductId,
    int OldQuantity,
    int NewQuantity
);