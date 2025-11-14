using Gateway.Api.Domain.Enums;

namespace Gateway.Api.Domain.DTOs;

public record UserResponse(
    Guid Id,
    string Email,
    string FullName,
    UserRole Role,
    DateTime CreatedAt
);