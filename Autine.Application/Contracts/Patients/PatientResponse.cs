namespace Autine.Application.Contracts.Patients;
public record PatientResponse (
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    DateTime BirthOfDate,
    string Gender,
    string Country,
    string City
    );
