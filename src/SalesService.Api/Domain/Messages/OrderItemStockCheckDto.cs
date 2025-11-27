namespace SalesService.Api.Domain.Messages;

public record OrderItemStockCheckDto(
    Guid ProductId,
    int Quantity
);