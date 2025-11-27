namespace SalesService.Api.Presentation.Contracts.Responses;

public record StockAvailabilityResponse(
    Guid Id,
    int AvailableQuantity
);