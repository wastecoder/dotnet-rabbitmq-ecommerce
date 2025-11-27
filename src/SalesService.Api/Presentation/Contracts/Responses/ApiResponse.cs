namespace SalesService.Api.Presentation.Contracts.Responses;

public record ApiResponse<T>
{
    public bool Success { get; init; }
    public T Data { get; init; }
    public string? Message { get; init; }
    public DateTime Timestamp { get; init; }
}