using SalesService.Api.Presentation.Contracts.Responses;

namespace SalesService.Api.Presentation.Factories;

public static class ApiResponseFactory
{
    public static ApiResponse<T> Success<T>(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message ?? "The operation was completed successfully."
        };
    }

    public static ApiResponse<T> Created<T>(T data)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = "The resource was created successfully."
        };
    }

    public static ApiResponse<T> Updated<T>(T data)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = "The resource was updated successfully."
        };
    }

    public static ApiResponse<T> Deleted<T>()
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = default!,
            Message = "The resource was deleted successfully."
        };
    }
}