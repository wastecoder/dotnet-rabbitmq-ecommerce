namespace SalesService.Api.Presentation.Contracts.Requests;

public record CreateOrderRequest(
    string Notes,
    List<OrderItemRequest> Items
);