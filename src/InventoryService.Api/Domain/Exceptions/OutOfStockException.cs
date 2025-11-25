namespace InventoryService.Api.Domain.Exceptions;

public class OutOfStockException : Exception
{
    public OutOfStockException(string message) : base(message) { }
}