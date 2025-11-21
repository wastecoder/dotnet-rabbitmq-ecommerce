namespace InventoryService.Api.Presentation.Contracts.Responses;

public record ApiResponse<T>
{
    public bool Success { get; init; }
    public T Data { get; init; }
    public string? Message { get; init; }
    public DateTime Timestamp { get; init; }

    public ApiResponse(bool success, T data, string? message = null, DateTime? timestamp = null)
    {
        Success = success;
        Data = data;
        Message = message;
        Timestamp = timestamp ?? DateTime.UtcNow;
    }
}