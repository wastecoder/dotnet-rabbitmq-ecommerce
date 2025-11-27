namespace SalesService.Api.Domain.Messages;

public record OrderItemMessage(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity
);