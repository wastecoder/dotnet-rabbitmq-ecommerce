using SalesService.Api.Domain.Enums;

namespace SalesService.Api.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }

    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Notes { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    public ICollection<OrderItem> Items { get; private set; }


    private Order() { }

    public Order(string notes)
    {
        Id = Guid.NewGuid();
        Status = OrderStatus.Pending;
        TotalAmount = 0;
        Notes = notes;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        IsDeleted = false;
        DeletedAt = null;
        Items = new List<OrderItem>();
    }


    public void UpdateNotes(string notes)
    {
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    private void RecalculateTotal()
    {
        TotalAmount = Items.Sum(i => i.TotalPrice);
    }

    public void AddItem(OrderItem item)
    {
        item.SetOrder(this);
        Items.Add(item);
        RecalculateTotal();
    }

    public void AddItems(IEnumerable<OrderItem> items)
    {
        foreach (var item in items)
        {
            item.SetOrder(this);
            Items.Add(item);
        }
        RecalculateTotal();
    }

    public void RemoveItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(x => x.Id == itemId);
        if (item == null) return;

        Items.Remove(item);
        RecalculateTotal();
    }

    public void UpdateItem(OrderItem item)
    {
        var existing = Items.FirstOrDefault(i => i.Id == item.Id);
        if (existing == null)
            throw new InvalidOperationException($"Item with Id {item.Id} not found in order.");

        existing.UpdateDetails(item.ProductName, item.UnitPrice, item.Quantity);
        RecalculateTotal();
    }

    public void SetStatusToConfirmed()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed.");

        Status = OrderStatus.Confirmed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetStatusToDelivered()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed orders can be delivered.");

        Status = OrderStatus.Delivered;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetStatusToCancelled()
    {
        switch (Status)
        {
            case OrderStatus.Delivered:
                throw new InvalidOperationException("Delivered orders cannot be cancelled.");

            case OrderStatus.Returned:
                throw new InvalidOperationException("Returned orders cannot be cancelled.");

            case OrderStatus.Cancelled:
                throw new InvalidOperationException("Order is already cancelled.");
        }

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetStatusToReturned()
    {
        if (Status != OrderStatus.Delivered)
            throw new InvalidOperationException("Only delivered orders can be returned.");

        Status = OrderStatus.Returned;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetStatusToFailed()
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Returned)
            throw new InvalidOperationException("Delivered or returned orders cannot fail.");

        Status = OrderStatus.Failed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DeletedAt.Value;
    }
}