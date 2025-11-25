using AutoMapper;
using InventoryService.Api.Domain.Entities;
using InventoryService.Api.Presentation.Contracts.Responses;

namespace InventoryService.Api.Application.Mapping;

public class StockMappingProfile : Profile
{
    public StockMappingProfile()
    {
        CreateMap<Product, StockAvailabilityResponse>()
            .ConstructUsing(src => new StockAvailabilityResponse(
                src.Id,
                src.Quantity
            ));

        CreateMap<(Product product, int oldQuantity), StockUpdatedResponse>()
            .ConstructUsing(src => new StockUpdatedResponse(
                src.product.Id,
                src.oldQuantity,
                src.product.Quantity
            ));
    }
}