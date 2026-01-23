using Gateway.Api.Application.Services;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Configurations;
using Gateway.Api.Infrastructure.Database;
using Gateway.Api.Infrastructure.Messaging;
using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Infrastructure.Messaging.Consumers;
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

        services.AddSingleton<RabbitMqConnection>();

        services.AddScoped<IRabbitMqConsumer<StockUpdatedEvent>, StockUpdatedConsumer>();

        services.AddHostedService(sp =>
            new RabbitMqBackgroundService<StockUpdatedEvent>(
                sp.GetRequiredService<RabbitMqConnection>(),
                sp.GetRequiredService<IServiceScopeFactory>(),
                queueName: "gateway.metrics.stock",
                routingKeys: ["stock.updated"]
            ));

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