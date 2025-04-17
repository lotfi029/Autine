namespace Autine.Application.Contracts.Patients;

public record UpdatePatientRequest(
    string FirstName,
    string LastName,
    string Bio,
    DateTime DateOfBirth,
    string Password,
    string Country,
    string City
);
