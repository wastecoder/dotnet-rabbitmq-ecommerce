using InventoryService.Api.Application.Services;
using InventoryService.Api.Domain.Interfaces;
using InventoryService.Api.Infrastructure.Configurations;
using InventoryService.Api.Infrastructure.Database;
using InventoryService.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Api.Application.Configuration;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInventoryInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register RabbitMQ settings
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));

        services.AddDbContext<InventoryDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("InventoryDb"));
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();

        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IStockService, StockService>();

        return services;
    }

    public static WebApplication SeedDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        db.Database.Migrate();
        InventoryDbSeeder.SeedAsync(db).Wait();

        return app;
    }
}