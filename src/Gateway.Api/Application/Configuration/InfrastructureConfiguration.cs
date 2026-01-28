using Gateway.Api.Application.Services;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Configurations;
using Gateway.Api.Infrastructure.Database;
using Gateway.Api.Infrastructure.Messaging.Consumers;
using Gateway.Api.Infrastructure.Messaging.Hosting;
using Gateway.Api.Infrastructure.Messaging.RabbitMq;
using Gateway.Api.Infrastructure.Repositories;
using Gateway.Api.Presentation.Integration.Events.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Application.Configuration;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddGatewayInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("GatewayDb"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IProductMetricsService, ProductMetricsService>();
        services.AddScoped<IProductSalesStatsService, ProductSalesStatsService>();
        services.AddScoped<ISalesCountersService, SalesCountersService>();

        services.AddScoped<IProductMetricsRepository, ProductMetricsRepository>();
        services.AddScoped<IProductSalesStatsRepository, ProductSalesStatsRepository>();
        services.AddScoped<ISalesMetricsRepository, SalesMetricsRepository>();
        services.AddScoped<IStockAlertRepository, StockAlertRepository>();

        services.AddSingleton<RabbitMqConnection>();
        services.AddGatewayMessaging();

        return services;
    }

    public static WebApplication ApplyMigrationsAndSeed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.Migrate();
        DbSeeder.SeedAsync(db).Wait();

        return app;
    }
}