using SalesService.Api.Application.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerDocumentation();
// builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSalesInfrastructure(builder.Configuration);
builder.Services.AddMappingConfiguration();
builder.Services.AddHttpClientsConfiguration(builder.Configuration);

var app = builder.Build();

// Database
app.SeedDatabase();

// Pipeline
app.UseApiConfiguration();
app.UseSwaggerDocumentation();

app.Run();
