using InventoryService.Api.Presentation.Contracts.Responses;

namespace InventoryService.Api.Presentation.Factories;

public static class ApiResponseFactory
{
    public static ApiResponse<T> Success<T>(T data, string? message = null)
    {
        return new ApiResponse<T>(
            success: true,
            data: data,
            message: message ?? "The operation was completed successfully.");
    }

    public static ApiResponse<T> Created<T>(T data)
    {
        return new ApiResponse<T>(
            success: true,
            data: data,
            message: "The resource was created successfully.");
    }

    public static ApiResponse<T> Updated<T>(T data)
    {
        return new ApiResponse<T>(
            success: true,
            data: data,
            message: "The resource was updated successfully.");
    }

    public static ApiResponse<T> Deleted<T>()
    {
        return new ApiResponse<T>(
            success: true,
            data: default!,
            message: "The resource was deleted successfully.");
    }
}