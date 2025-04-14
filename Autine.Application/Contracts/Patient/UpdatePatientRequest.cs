namespace Autine.Application.Contracts.Patient;

public record UpdatePatientRequest(
    string FirstName,
    string LastName,
    string Bio,
    DateTime DateOfBirth,
    string Password,
    string Country,
    string City
);
