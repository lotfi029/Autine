namespace Autine.Application.Contracts.Auths;
public record AuthResponse(
    string AccessToken,
    int ExpiresIn,
    string TokenType = "Berear"
);