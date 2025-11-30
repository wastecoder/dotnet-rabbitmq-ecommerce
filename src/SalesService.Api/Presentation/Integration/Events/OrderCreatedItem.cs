namespace SalesService.Api.Presentation.Integration.Events;

public record OrderCreatedItem(
    Guid ProductId,
    int Quantity
);