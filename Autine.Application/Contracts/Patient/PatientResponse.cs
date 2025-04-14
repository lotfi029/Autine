namespace Autine.Application.Contracts.Patient;
public record PatientResponse (
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    DateTime BirthOfDate,
    string Gender,
    string Country,
    string City,
    bool IsSupervised
    );
