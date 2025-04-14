namespace Autine.Application.Contracts.Auth;

public record CreateSupervisorRequest(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string Gender,
    string? Bio,
    IFormFile? ProfilePic,
    string Country,
    string City,
    string SuperviorRole,
    DateTime DateOfBirth
);

