namespace Autine.Application.Contracts.Auths;
public record RegisterRequest (
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string Gender,
    string? Bio, 
    IFormFile? ProfilePic, 
    string? Country,
    string? City,
    DateTime DateOfBirth
);

