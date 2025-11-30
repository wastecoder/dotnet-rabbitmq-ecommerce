using SalesService.Api.Application.Mapping;

namespace SalesService.Api.Application.Configuration;

public static class MappingConfiguration
{
    public static IServiceCollection AddMappingConfiguration(
        this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<OrderMappingProfile>();
        }, typeof(OrderMappingProfile).Assembly);

        return services;
    }
}