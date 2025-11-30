using System.Text.Json.Serialization;
using FluentValidation;
using SalesService.Api.Application.Validation;
using SalesService.Api.Presentation.Middlewares;

namespace SalesService.Api.Application.Configuration;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiConfiguration(
        this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

        services.AddEndpointsApiExplorer();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static WebApplication UseApiConfiguration(
        this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.MapControllers();

        return app;
    }
}