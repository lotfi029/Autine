namespace Autine.Application.Contracts.Patient;

public record PatientRequest(
    string FirstName,
    string LastName,
    string Country,
    string City,
    string Gender,
    string Bio,
    DateTime DateOfBirth,
    string Email,
    string UserName,
    string Password
    );