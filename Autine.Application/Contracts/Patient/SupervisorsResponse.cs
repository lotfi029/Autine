namespace Autine.Application.Contracts.Patient;

public record SupervisorsResponse(
    string Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Gender,
    string Role,
    bool IsSupervised
    );