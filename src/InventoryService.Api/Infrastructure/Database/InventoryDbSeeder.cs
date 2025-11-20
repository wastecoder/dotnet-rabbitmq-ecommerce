using InventoryService.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Api.Infrastructure.Database;

public static class InventoryDbSeeder
{
    public static async Task SeedAsync(InventoryDbContext db)
    {
        // Prevent duplicate seeding
        if (await db.Products.AnyAsync())
            return;

        var products = new List<Product>
        {
            new Product(
                name: "Gaming Mouse",
                price: 199.90m,
                quantity: 50,
                description: "High precision RGB gaming mouse."
            ),

            new Product(
                name: "Ergonomic Mouse",
                price: 249.90m,
                quantity: 40,
                description: "Ergonomic vertical mouse designed to reduce wrist strain."
            ),

            new Product(
                name: "Mechanical Keyboard",
                price: 349.99m,
                quantity: 30,
                description: "Tenkeyless mechanical keyboard with red switches."
            ),

            new Product(
                name: "Split Keyboard",
                price: 499.00m,
                quantity: 20,
                description: "Ergonomic split mechanical keyboard for improved typing posture."
            ),

            new Product(
                name: "Gamer Monitor 27\" 165Hz",
                price: 1899.90m,
                quantity: 15,
                description: "27-inch gaming monitor with 165Hz refresh rate and 1ms response time."
            ),

            new Product(
                name: "USB-C Charger 65W",
                price: 129.90m,
                quantity: 100,
                description: "Fast charger compatible with smartphones and laptops."
            )
        };

        db.Products.AddRange(products);
        await db.SaveChangesAsync();
    }
}