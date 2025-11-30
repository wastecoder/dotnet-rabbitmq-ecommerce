using Microsoft.EntityFrameworkCore;
using SalesService.Api.Application.Orchestrators;
using SalesService.Api.Application.Services;
using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Infrastructure.Configurations;
using SalesService.Api.Infrastructure.Database;
using SalesService.Api.Infrastructure.Messaging;
using SalesService.Api.Infrastructure.Repositories;

namespace SalesService.Api.Application.Configuration;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddSalesInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
        services.AddSingleton<IRabbitMqProducer, RabbitMqPublisher>();

        services.AddDbContext<SalesDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("SalesDb"));
        });

        // Repositories
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Services
        services.AddScoped<IOrderService, OrderService>();
        
        // Orchestrators
        services.AddScoped<IOrderOrchestrator, OrderOrchestrator>();
        services.AddScoped<IStockOrchestrator, StockOrchestrator>();

        return services;
    }

    public static WebApplication SeedDatabase(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SalesDbContext>();

        db.Database.Migrate();
        SalesDbSeeder.SeedAsync(db).Wait();

        return app;
    }
}