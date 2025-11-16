namespace Gateway.Api.Domain.DTOs;

public record AuthResult(bool Success, TokenResult? Token, ErrorResponse? Error);