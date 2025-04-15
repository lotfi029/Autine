namespace Autine.Application.Errors;

public class PatientErrors
{
    public static readonly Error PatientsNotFound
        = Error.NotFound($"Patient.{nameof(PatientsNotFound)}", "Patients not found");
    public static readonly Error DuplicatedPatient
        = Error.Conflict($"Patient.{nameof(DuplicatedPatient)}", "this patient is already supervised by you.");
}