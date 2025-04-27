namespace Autine.Application.Contracts.Patients;
public record PatientResponse (
    string Id,
    string FirstName,
    string LastName,
    string UserName,
    DateTime BirthOfDate,
    string Gender,
    string Country,
    string City,
    DateTime CreatedAt,
    string Image
    );
