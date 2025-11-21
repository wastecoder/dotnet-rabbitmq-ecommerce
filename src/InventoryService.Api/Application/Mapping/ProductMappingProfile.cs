using AutoMapper;
using InventoryService.Api.Domain.Entities;
using InventoryService.Api.Presentation.Contracts.Requests;
using InventoryService.Api.Presentation.Contracts.Responses;

namespace InventoryService.Api.Application.Mapping;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<ProductRequest, Product>();
        CreateMap<Product, ProductResponse>();
    }
}