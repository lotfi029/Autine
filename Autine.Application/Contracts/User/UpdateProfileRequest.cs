namespace Autine.Application.Contracts.User;

public record UpdateProfileRequest(
    string FirstName,
    string LastName,
    string Bio,
    string? Country,
    string? City
    );
