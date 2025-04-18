namespace Autine.Application.Contracts.Users;

public record UpdateProfileRequest(
    string FirstName,
    string LastName,
    string Bio,
    string? Country,
    string? City
    );
