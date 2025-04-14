namespace Autine.Application.Contracts.User;

public record UserProfileResponse(
    string FirstName,
    string LastName,
    string Bio,
    string Gender,
    string? Country,
    string? City,
    DateTime DateOfBirth
    );





