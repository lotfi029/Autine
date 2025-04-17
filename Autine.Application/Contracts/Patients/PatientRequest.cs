namespace Autine.Application.Contracts.Patients;

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