namespace Autine.Application.Contracts.Patients;

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    string Bio,
    string Country,
    string City
);
