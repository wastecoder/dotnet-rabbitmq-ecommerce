namespace InventoryService.Api.Domain.Enums;

public enum ErrorType
{
    ValidationError,
    Unauthorized,
    Forbidden,
    NotFound,
    Conflict,
    BadRequest,
    UpstreamServiceError,
    InternalServerError
}