using Gateway.Api.Application.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddGatewayInfrastructure(builder.Configuration);
builder.Services.AddCorsConfiguration(builder.Configuration);

var app = builder.Build();

// Database
app.ApplyMigrationsAndSeed();

// Pipeline
app.UseApiConfiguration();
app.UseSwaggerDocumentation();

app.Run();
