using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Infrastructure.Http;

namespace SalesService.Api.Application.Configuration;

public static class HttpClientsConfiguration
{
    public static IServiceCollection AddHttpClientsConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
        {
            client.BaseAddress = new Uri(
                configuration["Services:Inventory"]
                ?? "http://localhost:5001/"
            );
        });

        return services;
    }
}