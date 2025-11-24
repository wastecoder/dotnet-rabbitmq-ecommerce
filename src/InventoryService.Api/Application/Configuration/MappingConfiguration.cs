using AutoMapper;
using InventoryService.Api.Application.Mapping;

namespace InventoryService.Api.Application.Configuration;

public static class MappingConfiguration
{
    public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ProductMappingProfile>();
        }, typeof(ProductMappingProfile).Assembly);

        return services;
    }
}