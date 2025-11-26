using Microsoft.EntityFrameworkCore;
using SalesService.Api.Domain.Entities;

namespace SalesService.Api.Infrastructure.Database;

public static class SalesDbSeeder
{
    public static async Task SeedAsync(SalesDbContext db)
    {
        // Prevent duplicate seeding
        if (await db.Orders.AnyAsync())
            return;

        var order1 = CreateOrder1();
        var order2 = CreateOrder2();
        var order3 = CreateOrder3();

        db.Orders.AddRange(order1, order2, order3);
        await db.SaveChangesAsync();
    }

    // Order 1 >>> 1 item
    private static Order CreateOrder1()
    {
        var order = new Order(notes: "First test order");

        var item = new OrderItem(
            productId: Guid.NewGuid(),
            productName: "Gaming Mouse",
            unitPrice: 199.90m,
            quantity: 2
        );

        order.AddItem(item);
        return order;
    }

    // Order 2 >>> 2 items
    private static Order CreateOrder2()
    {
        var order = new Order(notes: "Second test order");

        var item1 = new OrderItem(
            productId: Guid.NewGuid(),
            productName: "Mechanical Keyboard",
            unitPrice: 349.99m,
            quantity: 1
        );

        var item2 = new OrderItem(
            productId: Guid.NewGuid(),
            productName: "USB-C Charger 65W",
            unitPrice: 129.90m,
            quantity: 3
        );

        order.AddItem(item1);
        order.AddItem(item2);
        return order;
    }

    // Order 3 >>> 3 items
    private static Order CreateOrder3()
    {
        var order = new Order(notes: "Third test order");

        var item1 = new OrderItem(
            productId: Guid.NewGuid(),
            productName: "Ergonomic Mouse",
            unitPrice: 249.90m,
            quantity: 1
        );

        var item2 = new OrderItem(
            productId: Guid.NewGuid(),
            productName: "Split Keyboard",
            unitPrice: 499.00m,
            quantity: 2
        );

        var item3 = new OrderItem(
            productId: Guid.NewGuid(),
            productName: "Gamer Monitor 27\" 165Hz",
            unitPrice: 1899.90m,
            quantity: 1
        );

        order.AddItem(item1);
        order.AddItem(item2);
        order.AddItem(item3);
        return order;
    }
}