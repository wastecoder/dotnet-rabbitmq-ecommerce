using Gateway.Api.Domain.Enums;

namespace Gateway.Api.Domain.DTOs;

public record CreateUserRequest(
    string Email,
    string Password,
    string FullName,
    UserRole Role
);