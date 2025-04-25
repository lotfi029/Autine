namespace Autine.Application.Errors;

public class BotPatientError
{
    public static readonly Error PatientNotFound
        = Error.NotFound($"BotUser.{nameof(PatientNotFound)}", "BotPatient not found.");
    
}
