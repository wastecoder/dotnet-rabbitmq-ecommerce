using Gateway.Api.Domain.Enums;

namespace Gateway.Api.Presentation.Contracts.Responses;

public record UserResponse(
    Guid Id,
    string Email,
    string FullName,
    UserRole Role,
    DateTime CreatedAt
);