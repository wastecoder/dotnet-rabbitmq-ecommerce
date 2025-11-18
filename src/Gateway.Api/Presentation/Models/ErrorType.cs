namespace Gateway.Api.Presentation.Models;

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