using System.Text.Json.Serialization;
using FluentValidation;
using InventoryService.Api.Application.Validation;
using InventoryService.Api.Presentation.Middlewares;

namespace InventoryService.Api.Application.Configuration;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddValidatorsFromAssemblyContaining<ProductRequestValidator>();

        services.AddEndpointsApiExplorer();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static WebApplication UseApiConfiguration(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.MapControllers();

        return app;
    }
}