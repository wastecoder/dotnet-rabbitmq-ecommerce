using AutoMapper;
using SalesService.Api.Domain.Entities;
using SalesService.Api.Presentation.Contracts.Requests;
using SalesService.Api.Presentation.Contracts.Responses;

namespace SalesService.Api.Application.Mapping;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<CreateOrderRequest, Order>();
        CreateMap<OrderItemRequest, OrderItem>();

        CreateMap<OrderItem, OrderItemResponse>()
            .ConstructUsing(src => new OrderItemResponse(
                src.ProductId,
                src.ProductName,
                src.UnitPrice,
                src.Quantity,
                src.TotalPrice
            ));

        CreateMap<Order, OrderResponse>()
            .ConstructUsing(src => new OrderResponse(
                src.Id,
                src.Notes,
                src.TotalAmount,
                src.Status,
                src.Items.Select(i => new OrderItemResponse(
                    i.ProductId,
                    i.ProductName,
                    i.UnitPrice,
                    i.Quantity,
                    i.TotalPrice
                )).ToList(),
                src.CreatedAt,
                src.UpdatedAt
            ));
    }
}