using Microsoft.EntityFrameworkCore;
using SalesService.Api.Application.Orchestrators;
using SalesService.Api.Application.Services;
using SalesService.Api.Domain.Exceptions;
using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Infrastructure.Database;
using SalesService.Api.Infrastructure.Http;
using SalesService.Api.Infrastructure.Repositories;
using SalesService.Api.Presentation.Contracts.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SalesDb")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IStockOrchestrator, StockOrchestrator>();
builder.Services.AddScoped<IOrderOrchestrator, OrderOrchestrator>();

builder.Services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/");
});

var app = builder.Build();

// Run database seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SalesDbContext>();
    await SalesDbSeeder.SeedAsync(db);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Test endpoint
app.MapGet("/", () => "Sales API is running...");

app.MapPost("/test/orders/create", async (
    CreateOrderRequest request,
    IOrderService orderService) =>
{
    try
    {
        var order = await orderService.CreateOrderAsync(request);
        return Results.Ok(order);
    }
    catch (BusinessValidationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPut("/test/orders/update/{id:guid}", async (
    Guid id,
    CreateOrderRequest request,
    IOrderService orderService) =>
{
    try
    {
        var order = await orderService.UpdateOrderAsync(id, request);
        return Results.Ok(order);
    }
    catch (NotFoundException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (BusinessValidationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/test/orders/{id:guid}", async (
    Guid id,
    IOrderService orderService) =>
{
    try
    {
        var order = await orderService.GetOrderByIdAsync(id);
        return Results.Ok(order);
    }
    catch (NotFoundException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
});

app.MapGet("/test/orders", async (IOrderService orderService) =>
{
    var orders = await orderService.GetAllOrdersAsync();
    return Results.Ok(orders);
});

app.MapDelete("/test/orders/{id:guid}", async (
    Guid id,
    IOrderService orderService) =>
{
    try
    {
        await orderService.SoftDeleteOrderAsync(id);
        return Results.Ok(new { message = "Order deleted." });
    }
    catch (NotFoundException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
});

app.Run();
