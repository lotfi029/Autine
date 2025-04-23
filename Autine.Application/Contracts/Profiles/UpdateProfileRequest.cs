namespace Autine.Application.Contracts.Profiles;

public record UpdateProfileRequest(
    string FirstName,
    string LastName,
    string Bio,
    string? Country,
    string? City
    );
