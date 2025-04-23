namespace Autine.Application.Contracts.Auths;

public record InternalRegisterResponse(string Code, string UserId, string Email, string HashPassword);