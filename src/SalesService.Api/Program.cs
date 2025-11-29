using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesService.Api.Application.Mapping;
using SalesService.Api.Application.Orchestrators;
using SalesService.Api.Application.Services;
using SalesService.Api.Application.Validation;
using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Infrastructure.Database;
using SalesService.Api.Infrastructure.Http;
using SalesService.Api.Infrastructure.Repositories;
using SalesService.Api.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add essential services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SalesDb")));

// Register repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register application services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderOrchestrator, OrderOrchestrator>();
builder.Services.AddScoped<IStockOrchestrator, StockOrchestrator>();

// Register AutoMapper
builder.Services.AddAutoMapper(
    cfg => cfg.AddProfile<OrderMappingProfile>(),
    typeof(OrderMappingProfile).Assembly
);

// Register validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Register exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Register Http Clients
builder.Services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:Inventory"] ?? "http://localhost:5001/");
});

var app = builder.Build();

// Run database seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SalesDbContext>();
    await SalesDbSeeder.SeedAsync(db);
}

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();
