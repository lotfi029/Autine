namespace Autine.Application.Contracts.Patient;

public record AssignPatientRequest(
    string PatientId,
    string AssigneeId
    );
