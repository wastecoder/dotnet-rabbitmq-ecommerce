namespace SalesService.Api.Domain.Messages;

public record OrderItemStockUpdateDto(
    Guid ProductId,
    int Quantity
);