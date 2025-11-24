namespace InventoryService.Api.Domain.Enums;

public enum ErrorType
{
    ValidationError,
    Unauthorized,
    Forbidden,
    NotFound,
    Conflict,
    UpstreamServiceError,
    InternalServerError
}