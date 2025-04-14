namespace Autine.Application.Contracts.Auth;

public record TokenRequest (
    string Email,
    string Password
);
