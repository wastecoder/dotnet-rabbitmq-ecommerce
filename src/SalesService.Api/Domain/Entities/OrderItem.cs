using System.Text.Json.Serialization;

namespace SalesService.Api.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    [JsonIgnore]
    public Order Order { get; private set; }

    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }

    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalPrice => UnitPrice * Quantity;


    private OrderItem() { }

    public OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }


    public void UpdateDetails(string productName, decimal unitPrice, int quantity)
    {
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void SetOrder(Order order)
    {
        Order = order;
        OrderId = order.Id;
    }
}