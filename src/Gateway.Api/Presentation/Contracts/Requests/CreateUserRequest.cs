using Gateway.Api.Domain.Enums;

namespace Gateway.Api.Presentation.Contracts.Requests;

public record CreateUserRequest(
    string Email,
    string Password,
    string FullName,
    UserRole Role
);