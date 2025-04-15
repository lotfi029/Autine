namespace Autine.Application.Contracts.Auth;
public record AuthResponse(
    string AccessToken,
    int ExpiresIn,
    string TokenType = "Berear"
);