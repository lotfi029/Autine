namespace Autine.Application.Contracts.Users;

public record UserProfileResponse(
    string FirstName,
    string LastName,
    string Bio,
    string Gender,
    string? Country,
    string? City,
    DateTime DateOfBirth
    );