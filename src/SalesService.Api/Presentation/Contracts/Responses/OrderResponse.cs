using SalesService.Api.Domain.Enums;

namespace SalesService.Api.Presentation.Contracts.Responses;

public record OrderResponse(
    Guid Id,
    string Notes,
    decimal TotalAmount,
    OrderStatus Status,
    List<OrderItemResponse> Items,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);