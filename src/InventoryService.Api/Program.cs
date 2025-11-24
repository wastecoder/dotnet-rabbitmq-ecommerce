using FluentValidation;
using FluentValidation.AspNetCore;
using InventoryService.Api.Application.Mapping;
using InventoryService.Api.Application.Services;
using InventoryService.Api.Application.Validation;
using InventoryService.Api.Domain.Interfaces;
using InventoryService.Api.Infrastructure.Database;
using InventoryService.Api.Infrastructure.Repositories;
using InventoryService.Api.Presentation.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("InventoryDb")));

// Register controllers
builder.Services.AddControllers();

// Register application services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Register AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProductMappingProfile>(), typeof(ProductMappingProfile).Assembly);

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<ProductRequestValidator>();

// Register exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Run database seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    await InventoryDbSeeder.SeedAsync(db);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

// Use controllers
app.MapControllers();

app.Run();
