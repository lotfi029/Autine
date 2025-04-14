namespace Autine.Application.Features.Patient.Add;
public record AddPatientCommand(string UserId, RegisterRequest Request) : ICommand;
