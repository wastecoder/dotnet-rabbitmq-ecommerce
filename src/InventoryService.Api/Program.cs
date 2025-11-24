using InventoryService.Api.Application.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddInventoryInfrastructure(builder.Configuration);
builder.Services.AddMappingConfiguration();

var app = builder.Build();

// Database
app.SeedDatabase();

// Pipeline
app.UseApiConfiguration();
app.UseSwaggerDocumentation();

app.Run();
