namespace SalesService.Api.Presentation.Contracts.Requests;

public record OrderItemRequest(
    Guid ProductId,
    int Quantity
);