using InventoryService.Api.Presentation.Contracts.Requests;

namespace InventoryService.Api.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }


    private Product() { }

    public Product(string name, decimal price, int quantity, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Quantity = quantity;
        Description = description;
        IsDeleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Update(ProductRequest request)
    {
        Name = request.Name;
        Price = request.Price;
        Quantity = request.Quantity;
        Description = request.Description ?? string.Empty;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}