using Gateway.Api.Application.Services;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Application.Configuration;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddGatewayInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("GatewayDb"));
        });

        services.AddScoped<ITokenService, TokenService>();

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